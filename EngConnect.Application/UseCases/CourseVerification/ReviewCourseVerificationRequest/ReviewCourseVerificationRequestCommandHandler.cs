using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
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