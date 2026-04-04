using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.BuildingBlock.EventBus.Constants;
using EngConnect.BuildingBlock.EventBus.Events;
using EngConnect.BuildingBlock.EventBus.Utils;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.CourseVerification.ReviewCourseVerificationRequest
{
    public class ReviewCourseVerificationRequestCommandHandler
        : ICommandHandler<ReviewCourseVerificationRequestCommand>
    {
        private readonly ILogger<ReviewCourseVerificationRequestCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ReviewCourseVerificationRequestCommandHandler(
            ILogger<ReviewCourseVerificationRequestCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            ReviewCourseVerificationRequestCommand command,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Start ReviewCourseVerificationRequestCommandHandler: {@Command}",
                command);

            try
            {
                var requestRepo = _unitOfWork.GetRepository<CourseVerificationRequest, Guid>();
                var courseRepo = _unitOfWork.GetRepository<Course, Guid>();
                var tutorRepo = _unitOfWork.GetRepository<Tutor, Guid>();
                var userRepo = _unitOfWork.GetRepository<User, Guid>();
                var outboxRepo = _unitOfWork.GetRepository<OutboxEvent, Guid>();

                var request = await requestRepo.FindFirstAsync(r => r.Id == command.Request.RequestId);

                if (request is null)
                {
                    return Result.Failure(HttpStatusCode.NotFound,
                        CourseErrors.VerificationRequestNotFound());
                }

                if (request.Status != "pending")
                {
                    return Result.Failure(HttpStatusCode.BadRequest,
                        CourseErrors.VerificationRequestAlreadyReviewed());
                }

                if (!command.Request.Approved && string.IsNullOrWhiteSpace(command.Request.RejectionReason))
                {
                    return Result.Failure(HttpStatusCode.BadRequest, CourseErrors.InvalidRejectionReason());
                }

                request.Status = command.Request.Approved ? "approved" : "rejected";
                request.ReviewedBy = command.Request.AdminUserId;
                request.ReviewedAt = DateTime.UtcNow;
                request.RejectionReason = command.Request.Approved
                    ? null
                    : command.Request.RejectionReason;

                // Update course.Status
                var course = await courseRepo.FindFirstAsync(c => c.Id == request.CourseId);

                if (course is not null)
                {
                    course.Status = command.Request.Approved ? "published" : "rejected";

                    var tutor = await tutorRepo.FindByIdAsync(course.TutorId, tracking: false,
                        cancellationToken: cancellationToken);

                    if (tutor is not null)
                    {
                        var tutorUser = await userRepo.FindByIdAsync(tutor.UserId, tracking: false,
                            cancellationToken: cancellationToken);

                        if (tutorUser is not null)
                        {
                            var reviewedEvent = CourseVerificationReviewedEvent.Create(
                                adminUserId: command.Request.AdminUserId,
                                courseId: course.Id,
                                tutorEmail: tutorUser.Email,
                                tutorFullName: $"{tutorUser.FirstName} {tutorUser.LastName}",
                                courseTitle: course.Title,
                                status: request.Status!,
                                rejectionReason: request.RejectionReason);

                            var notificationEvent = NotificationHelper.CreateNotification(
                                reviewedEvent,
                                [tutorUser.Id],
                                [nameof(UserRoleEnum.Tutor)],
                                nameof(Channel.Email));

                            outboxRepo.Add(OutboxEvent.CreateOutboxEvent(nameof(Course), course.Id,
                                notificationEvent));
                        }
                    }
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation(
                    "End ReviewCourseVerificationRequestCommandHandler: {RequestId}",
                    request.Id);

                return Result.Success(request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred in ReviewCourseVerificationRequestCommandHandler {@Message}",
                    ex.Message);

                return Result.Failure<Guid>(
                    HttpStatusCode.InternalServerError,
                    CommonErrors.InternalServerError());
            }
        }
    }
}