using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.TutorSchedules.UpdateTutorSchedule;

public sealed class UpdateTutorScheduleCommandHandler : ICommandHandler<UpdateTutorScheduleCommand>
{
    private readonly ILogger<UpdateTutorScheduleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTutorScheduleCommandHandler(
        ILogger<UpdateTutorScheduleCommandHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateTutorScheduleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateTutorScheduleCommandHandler: {@Command}", command);

        try
        {
            var repo = _unitOfWork.GetRepository<TutorSchedule, Guid>();
            var entity = await repo.FindByIdAsync(command.Request.Id, cancellationToken: cancellationToken);

            if (entity is null)
            {
                return Result.Failure(HttpStatusCode.NotFound, ScheduleErrors.TutorScheduleNotFound());
            }

            entity.Weekday = command.Request.Weekday;
            entity.StartTime = command.Request.StartTime;
            entity.EndTime = command.Request.EndTime;

            repo.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdateTutorScheduleCommandHandler: {ScheduleId}", entity.Id);
            return Result.Success(entity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateTutorScheduleCommandHandler: {Message}", ex.Message);
            return Result.Failure<Guid>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}