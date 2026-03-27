using System.Net;
using EngConnect.Application.UseCases.TutorSchedules.Extensions;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest;

public sealed class UpdateLessonRescheduleRequestCommandHandler : ICommandHandler<UpdateLessonRescheduleRequestCommand>
{
    private readonly ILogger<UpdateLessonRescheduleRequestCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLessonRescheduleRequestCommandHandler(
        ILogger<UpdateLessonRescheduleRequestCommandHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateLessonRescheduleRequestCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateLessonRescheduleRequestCommandHandler: {@Command}", command);

        try
        {
            return await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                var requestRepo = _unitOfWork.GetRepository<LessonRescheduleRequest, Guid>();
                var lessonRepo = _unitOfWork.GetRepository<Lesson, Guid>();

                var request = await requestRepo.FindByIdAsync(command.Request.Id, tracking: true, cancellationToken: cancellationToken);

                if (request is null)
                {
                    return Result.Failure(HttpStatusCode.NotFound, ScheduleErrors.RescheduleRequestNotFound());
                }

                if (!ScheduleStatusExtensions.IsPendingLessonRescheduleRequestStatus(request.Status))
                {
                    return Result.Failure(HttpStatusCode.BadRequest, ScheduleErrors.RescheduleRequestAlreadyFinalized());
                }

                if (!ScheduleStatusExtensions.IsValidLessonRescheduleRequestStatus(command.Request.Status))
                {
                    return Result.Failure(HttpStatusCode.BadRequest,
                        ScheduleErrors.InvalidRescheduleStatus(command.Request.Status ?? string.Empty));
                }

                // Approve
                if (command.Request.Status == nameof(LessonRescheduleRequestStatus.Approved))
                {
                    if (request.ProposedStartTime >= request.ProposedEndTime)
                    {
                        return Result.Failure(HttpStatusCode.BadRequest, ScheduleErrors.InvalidTimeRange());
                    }

                    if (request.ProposedStartTime <= DateTime.UtcNow)
                    {
                        return Result.Failure(HttpStatusCode.BadRequest, ScheduleErrors.ProposedTimeMustBeInFuture());
                    }

                    var lesson = await lessonRepo.FindByIdAsync(request.LessonId, tracking: true, cancellationToken: cancellationToken);

                    if (lesson is null)
                    {
                        return Result.Failure(HttpStatusCode.NotFound, ScheduleErrors.LessonNotFound());
                    }

                    // No overlap
                    var hasConflict = await lessonRepo.AnyAsync(l =>
                            l.Id != lesson.Id
                            && l.StartTime != null
                            && l.EndTime != null
                            && l.StartTime < request.ProposedEndTime
                            && l.EndTime > request.ProposedStartTime,
                        cancellationToken);

                    if (hasConflict)
                    {
                        return Result.Failure(HttpStatusCode.BadRequest, ScheduleErrors.LessonTimeConflict());
                    }

                    lesson.StartTime = request.ProposedStartTime;
                    lesson.EndTime = request.ProposedEndTime;

                    lessonRepo.Update(lesson);
                }

                request.Status = command.Request.Status;
                request.TutorNote = command.Request.TutorNote;

                requestRepo.Update(request);

                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("End UpdateLessonRescheduleRequestCommandHandler: {RequestId}", request.Id);

                return Result.Success(request.Id);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateLessonRescheduleRequestCommandHandler: {Message}", ex.Message);
            return Result.Failure<Guid>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}