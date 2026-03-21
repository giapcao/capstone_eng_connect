using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;

namespace EngConnect.Application.UseCases.TutorVerification.CreateTutorVerificationRequest
{
    public class CreateTutorVerificationRequestCommandHandler
        : ICommandHandler<CreateTutorVerificationRequestCommand>
    {
        private readonly ILogger<CreateTutorVerificationRequestCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTutorVerificationRequestCommandHandler(
            ILogger<CreateTutorVerificationRequestCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            CreateTutorVerificationRequestCommand command,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "Start CreateTutorVerificationRequestCommandHandler: {@Command}",
                command);

            try
            {
                var tutorRepo = _unitOfWork.GetRepository<Domain.Persistence.Models.Tutor, Guid>();
                var requestRepo = _unitOfWork.GetRepository<TutorVerificationRequest, Guid>();

                var tutor = await tutorRepo.FindFirstAsync(
                    t => t.Id == command.Request.TutorId,
                    cancellationToken: cancellationToken);

                if (ValidationUtil.IsNullOrEmpty(tutor))
                {
                    _logger.LogWarning("Tutor with ID {TutorId} not found when creating verification request.",
                        command.Request.TutorId);
                    return Result.Failure(HttpStatusCode.BadRequest, TutorErrors.TutorNotFound());
                }

                var pendingStatus = nameof(TutorVerificationRequestStatus.Pending);

                var hasPending = await requestRepo.AnyAsync(
                    r => r.TutorId == command.Request.TutorId && r.Status == pendingStatus,
                    cancellationToken);

                if (hasPending)
                {
                    return Result.Failure(
                        HttpStatusCode.BadRequest, TutorErrors.VerificationRequestAlreadyPending(command.Request.TutorId));
                }

                var entity = new TutorVerificationRequest
                {
                    TutorId = command.Request.TutorId,
                    Status = pendingStatus,
                    SubmittedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

                requestRepo.Add(entity);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation(
                    "End CreateTutorVerificationRequestCommandHandler: {RequestId}",
                    entity.Id);

                return Result.Success(entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred in CreateTutorVerificationRequestCommandHandler {@Message}",
                    ex.Message);

                return Result.Failure<Guid>(
                    HttpStatusCode.InternalServerError,
                    CommonErrors.InternalServerError());
            }
        }
    }
}