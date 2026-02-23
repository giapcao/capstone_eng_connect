using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EngConnect.Application.UseCases.TutorVerification.UpdateTutorVerificationRequest
{
    public class UpdateTutorVerificationRequestCommandHandler
    : ICommandHandler<UpdateTutorVerificationRequestCommand>
    {
        private readonly ILogger<UpdateTutorVerificationRequestCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTutorVerificationRequestCommandHandler(
            ILogger<UpdateTutorVerificationRequestCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            UpdateTutorVerificationRequestCommand command,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Start UpdateTutorVerificationRequestCommandHandler: {@Command}", command);

            try
            {
                var repo = _unitOfWork.GetRepository<TutorVerificationRequest, Guid>();

                var entity = await repo.FindByIdAsync(
                    command.Request.RequestId,
                    cancellationToken: cancellationToken);

                if (entity is null)
                {
                    return Result.Failure(
                        HttpStatusCode.NotFound,
                        TutorErrors.VerificationRequestNotFound(command.Request.RequestId));
                }

                entity.TutorId = command.Request.TutorId;
                entity.Status = command.Request.Status;
                entity.SubmittedAt = command.Request.SubmittedAt;
                entity.ReviewedAt = command.Request.ReviewedAt;
                entity.ReviewedBy = command.Request.ReviewedBy;
                entity.RejectionReason = command.Request.RejectionReason;

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("End UpdateTutorVerificationRequestCommandHandler: {RequestId}", entity.Id);

                return Result.Success(entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in UpdateTutorVerificationRequestCommandHandler {@Message}", ex.Message);

                return Result.Failure<Guid>(
                    HttpStatusCode.InternalServerError,
                    CommonErrors.InternalServerError());
            }
        }
    }
}