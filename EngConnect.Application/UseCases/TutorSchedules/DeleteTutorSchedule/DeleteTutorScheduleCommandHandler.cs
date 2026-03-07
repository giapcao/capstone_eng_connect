using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.TutorSchedules.DeleteTutorSchedule;

public sealed class DeleteTutorScheduleCommandHandler : ICommandHandler<DeleteTutorScheduleCommand>
{
    private readonly ILogger<DeleteTutorScheduleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTutorScheduleCommandHandler(
        ILogger<DeleteTutorScheduleCommandHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeleteTutorScheduleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteTutorScheduleCommandHandler: {@Command}", command);

        try
        {
            var repo = _unitOfWork.GetRepository<TutorSchedule, Guid>();
            var entity = await repo.FindByIdAsync(command.Id, cancellationToken: cancellationToken);

            if (entity is null)
            {
                return Result.Failure(HttpStatusCode.NotFound, ScheduleErrors.TutorScheduleNotFound());
            }

            repo.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End DeleteTutorScheduleCommandHandler: {ScheduleId}", entity.Id);
            return Result.Success(entity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteTutorScheduleCommandHandler: {Message}", ex.Message);
            return Result.Failure<Guid>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}