using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.BuildingBlock.EventBus.Constants;
using EngConnect.BuildingBlock.EventBus.Events;
using EngConnect.BuildingBlock.EventBus.Utils;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;

namespace EngConnect.Application.UseCases.TutorVerification.ReviewTutorVerificationRequest
{
    public class ReviewTutorVerificationRequestCommandHandler
        : ICommandHandler<ReviewTutorVerificationRequestCommand>
    {
        private readonly ILogger<ReviewTutorVerificationRequestCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ReviewTutorVerificationRequestCommandHandler(
            ILogger<ReviewTutorVerificationRequestCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            ReviewTutorVerificationRequestCommand command,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Start ReviewTutorVerificationRequestCommandHandler: {@Command}",
                command);

            try
            {
                var requestRepo = _unitOfWork.GetRepository<TutorVerificationRequest, Guid>();
                var tutorRepo = _unitOfWork.GetRepository<Domain.Persistence.Models.Tutor, Guid>();
                var userRepo = _unitOfWork.GetRepository<User, Guid>();
                var outboxRepo = _unitOfWork.GetRepository<OutboxEvent, Guid>();

                var request = await requestRepo.FindFirstAsync(
                    r => r.Id == command.Request.RequestId,
                    cancellationToken: cancellationToken);

                if (ValidationUtil.IsNullOrEmpty(request))
                {
                    return Result.Failure(
                        HttpStatusCode.NotFound,
                        TutorErrors.VerificationRequestNotFound(command.Request.RequestId));
                }

                if (!string.Equals(request.Status, nameof(TutorVerificationRequestStatus.Pending), StringComparison.Ordinal))
                {
                    return Result.Failure(
                        HttpStatusCode.BadRequest,
                        TutorErrors.VerificationRequestAlreadyReviewed(command.Request.RequestId));
                }

                request.Status = command.Request.Approved
                    ? nameof(TutorVerificationRequestStatus.Approved)
                    : nameof(TutorVerificationRequestStatus.Rejected);

                request.ReviewedBy = command.Request.AdminUserId;
                request.ReviewedAt = DateTime.UtcNow;
                request.RejectionReason = command.Request.Approved
                    ? null
                    : command.Request.RejectionReason;

                var tutor = await tutorRepo.FindFirstAsync(
                    t => t.Id == request.TutorId,
                    cancellationToken: cancellationToken);

                if (ValidationUtil.IsNotNullOrEmpty(tutor))
                {
                    tutor.VerifiedStatus = command.Request.Approved
                        ? nameof(TutorVerifiedStatus.Verified)
                        : nameof(TutorVerifiedStatus.Rejected);

                    var tutorUser = await userRepo.FindByIdAsync(
                        tutor.UserId,
                        tracking: false,
                        cancellationToken: cancellationToken);

                    if (tutorUser is not null)
                    {
                        var reviewedEvent = TutorVerificationReviewedEvent.Create(
                            adminUserId: command.Request.AdminUserId!.Value,
                            tutorId: tutor.Id,
                            tutorEmail: tutorUser.Email,
                            tutorFullName: $"{tutorUser.FirstName} {tutorUser.LastName}",
                            status: request.Status,
                            rejectionReason: request.RejectionReason);

                        var notificationEvent = NotificationHelper.CreateNotification(
                            reviewedEvent,
                            [tutorUser.Id],
                            [nameof(UserRoleEnum.Tutor)],
                            nameof(Channel.Email));

                        outboxRepo.Add(OutboxEvent.CreateOutboxEvent(
                            nameof(Tutor),
                            tutor.Id,
                            notificationEvent));
                    }
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation(
                    "End ReviewTutorVerificationRequestCommandHandler: {RequestId}",
                    request.Id);

                return Result.Success(request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred in ReviewTutorVerificationRequestCommandHandler {@Message}",
                    ex.Message);

                return Result.Failure<Guid>(
                    HttpStatusCode.InternalServerError,
                    CommonErrors.InternalServerError());
            }
        }
    }
}
