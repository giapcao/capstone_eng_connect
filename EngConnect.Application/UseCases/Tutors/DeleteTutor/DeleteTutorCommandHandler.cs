using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Tutors.DeleteTutor
{
    public class DeleteTutorCommandHandler : ICommandHandler<DeleteTutorCommand>
    {
        private readonly ILogger<DeleteTutorCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTutorCommandHandler(ILogger<DeleteTutorCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            DeleteTutorCommand command,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Start DeleteTutorCommandHandler: {@Command}", command);

            try
            {
                var repo = _unitOfWork.GetRepository<Domain.Persistence.Models.Tutor, Guid>();

                var entity = await repo
                    .FindAll()
                    .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

                if (entity is null)
                {
                    return Result.Failure<bool>(HttpStatusCode.NotFound, CommonErrors.NotFound<Domain.Persistence.Models.Tutor>("Tutor"));
                }

                repo.Delete(entity);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("End DeleteTutorCommandHandler");
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in DeleteTutorCommandHandler {@Message}", ex.Message);
                return Result.Failure<bool>(HttpStatusCode.InternalServerError,
                    CommonErrors.InternalServerError());
            }
        }
    }
}