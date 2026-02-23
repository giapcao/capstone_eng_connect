using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EngConnect.Application.UseCases.TutorVerification.DeleteTutorVerificationRequest
{
    public class DeleteTutorVerificationRequestCommandHandler
    : ICommandHandler<DeleteTutorVerificationRequestCommand>
    {
        private readonly ILogger<DeleteTutorVerificationRequestCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTutorVerificationRequestCommandHandler(
            ILogger<DeleteTutorVerificationRequestCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            DeleteTutorVerificationRequestCommand command,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Start DeleteTutorVerificationRequestCommandHandler: {@Command}", command);

            try
            {
                var repo = _unitOfWork.GetRepository<TutorVerificationRequest, Guid>();

                var entity = await repo.FindByIdAsync(command.RequestId, cancellationToken: cancellationToken);

                if (entity is null)
                {
                    return Result.Failure(
                        HttpStatusCode.NotFound,
                        TutorErrors.VerificationRequestNotFound(command.RequestId));
                }

                repo.Delete(entity);
                await _unitOfWork.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in DeleteTutorVerificationRequestCommandHandler {@Message}", ex.Message);

                return Result.Failure<Guid>(
                    HttpStatusCode.InternalServerError,
                    CommonErrors.InternalServerError());
            }
        }
    }
}