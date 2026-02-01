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

                var request = await requestRepo.FindFirstAsync(r => r.Id == command.Request.RequestId);

                if (request is null)
                {
                    return Result.Failure(HttpStatusCode.NotFound,
                        TutorErrors.VerificationRequestNotFound(command.Request.RequestId));
                }

                if (request.Status != "pending")
                {
                    return Result.Failure(HttpStatusCode.BadRequest,
                        TutorErrors.VerificationRequestAlreadyReviewed(command.Request.RequestId));
                }

                request.Status = command.Request.Approved ? "approved" : "rejected";
                request.ReviewedBy = command.Request.AdminUserId;
                request.ReviewedAt = DateTime.UtcNow;
                request.RejectionReason = command.Request.Approved
                    ? null
                    : command.Request.RejectionReason;

                // Update tutor.VerifiedStatus
                var tutor = await tutorRepo.FindFirstAsync(t => t.Id == request.TutorId);

                if (tutor is not null)
                {
                    tutor.VerifiedStatus = command.Request.Approved ? "verified" : "rejected";
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