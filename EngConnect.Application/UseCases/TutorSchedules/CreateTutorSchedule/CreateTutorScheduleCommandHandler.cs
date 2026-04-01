using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.TutorSchedules.CreateTutorSchedule;

public sealed class CreateTutorScheduleCommandHandler : ICommandHandler<CreateTutorScheduleCommand>
{
    private readonly ILogger<CreateTutorScheduleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTutorScheduleCommandHandler(
        ILogger<CreateTutorScheduleCommandHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateTutorScheduleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateTutorScheduleCommandHandler: {@Command}", command);

        try
        {
            var tutorRepo = _unitOfWork.GetRepository<Tutor, Guid>();
            var scheduleRepo = _unitOfWork.GetRepository<TutorSchedule, Guid>();

            var tutor = await tutorRepo.FindFirstAsync(t => t.Id == command.Request.TutorId, cancellationToken: cancellationToken);
            if (ValidationUtil.IsNullOrEmpty(tutor))
            {
                return Result.Failure(HttpStatusCode.BadRequest, ScheduleErrors.TutorNotFound());
            }

            var entity = new TutorSchedule
            {
                TutorId = command.Request.TutorId,
                Weekday = command.Request.Weekday,
                StartTime = command.Request.StartTime,
                EndTime = command.Request.EndTime
            };

            scheduleRepo.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End CreateTutorScheduleCommandHandler: {ScheduleId}", entity.Id);
            return Result.Success(entity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateTutorScheduleCommandHandler: {Message}", ex.Message);
            return Result.Failure<Guid>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}