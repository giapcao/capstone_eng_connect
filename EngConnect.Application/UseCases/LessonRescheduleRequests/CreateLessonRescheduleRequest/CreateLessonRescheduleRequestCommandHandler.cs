using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.LessonRescheduleRequests.CreateLessonRescheduleRequest;

public sealed class CreateLessonRescheduleRequestCommandHandler
    : ICommandHandler<CreateLessonRescheduleRequestCommand>
{
    private readonly ILogger<CreateLessonRescheduleRequestCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLessonRescheduleRequestCommandHandler(
        ILogger<CreateLessonRescheduleRequestCommandHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateLessonRescheduleRequestCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateLessonRescheduleRequestCommandHandler: {@Command}", command);

        try
        {
            var lessonRepo = _unitOfWork.GetRepository<Lesson, Guid>();
            var studentRepo = _unitOfWork.GetRepository<Student, Guid>();
            var repo = _unitOfWork.GetRepository<LessonRescheduleRequest, Guid>();

            var lesson = await lessonRepo.FindFirstAsync(x => x.Id == command.Request.LessonId, cancellationToken: cancellationToken);
            if (lesson is null)
            {
                return Result.Failure(HttpStatusCode.NotFound, ScheduleErrors.LessonNotFound());
            }

            var student = await studentRepo.FindFirstAsync(x => x.Id == command.Request.StudentId, cancellationToken: cancellationToken);
            if (student is null)
            {
                return Result.Failure(HttpStatusCode.NotFound, ScheduleErrors.StudentNotFound());
            }

            var previousLesson = await lessonRepo
                .FindAll(x => x.TutorId == lesson.TutorId
                              && x.Id != lesson.Id
                              && x.EndTime != null
                              && x.EndTime <= command.Request.ProposedStartTime,
                    cancellationToken: cancellationToken)
                .OrderByDescending(x => x.EndTime)
                .FirstOrDefaultAsync(cancellationToken);

            if (previousLesson?.EndTime != null && command.Request.ProposedStartTime < previousLesson.EndTime.Value.AddHours(1))
            {
                return Result.Failure(HttpStatusCode.BadRequest, ScheduleErrors.ProposedTimeMustHaveOneHourBuffer());
            }

            var nextLesson = await lessonRepo
                .FindAll(x => x.TutorId == lesson.TutorId
                              && x.Id != lesson.Id
                              && x.StartTime != null
                              && x.StartTime >= command.Request.ProposedEndTime,
                    cancellationToken: cancellationToken)
                .OrderBy(x => x.StartTime)
                .FirstOrDefaultAsync(cancellationToken);

            if (nextLesson?.StartTime != null && command.Request.ProposedEndTime > nextLesson.StartTime.Value.AddHours(-1))
            {
                return Result.Failure(HttpStatusCode.BadRequest, ScheduleErrors.ProposedTimeMustHaveOneHourBuffer());
            }

            var hasPending = await repo.AnyAsync(
                x => x.LessonId == command.Request.LessonId
                    && x.Status == nameof(LessonRescheduleRequestStatus.Pending),
                cancellationToken);

            if (hasPending)
            {
                return Result.Failure(HttpStatusCode.BadRequest, ScheduleErrors.PendingRescheduleRequestAlreadyExists());
            }

            var entity = new LessonRescheduleRequest
            {
                LessonId = command.Request.LessonId,
                StudentId = command.Request.StudentId,
                ProposedStartTime = command.Request.ProposedStartTime,
                ProposedEndTime = command.Request.ProposedEndTime,
                Status = nameof(LessonRescheduleRequestStatus.Pending),
                TutorNote = command.Request.TutorNote
            };

            repo.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End CreateLessonRescheduleRequestCommandHandler: {RequestId}", entity.Id);
            return Result.Success(entity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateLessonRescheduleRequestCommandHandler: {Message}", ex.Message);
            return Result.Failure<Guid>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}