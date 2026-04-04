using System.Net;
using EngConnect.Application.UseCases.Lessons.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Lessons.CreateLesson;

public class CreateLessonCommandHandler : ICommandHandler<CreateLessonCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateLessonCommandHandler> _logger;

    public CreateLessonCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateLessonCommandHandler> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateLessonCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateLessonCommandHandler: {@command}", command);
        try
        {
            var lesson = command.Adapt<Lesson>();

            var studentExists = await _unitOfWork.GetRepository<Student, Guid>()
                .AnyAsync(x => x.Id == lesson.StudentId, cancellationToken: cancellationToken);

            if (!studentExists)
            {
                _logger.LogWarning("Student not found with ID: {StudentId}", command.StudentId);
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.NotFound<Student>("thÃ´ng tin Há»c sinh."));
            }

            var trackingContext = await LessonTrackingContext.ResolveAsync(
                _unitOfWork,
                command.EnrollmentId,
                command.StudentId,
                command.ModuleId,
                command.SessionId,
                cancellationToken);

            if (trackingContext.IsFailure)
            {
                _logger.LogWarning(
                    "Invalid lesson tracking data for EnrollmentId {EnrollmentId}, ModuleId {ModuleId}, SessionId {SessionId}",
                    command.EnrollmentId,
                    command.ModuleId,
                    command.SessionId);
                return Result.Failure(trackingContext.HttpStatusCode, trackingContext.Error!);
            }

            lesson.TutorId = trackingContext.Data!.TutorId;
            lesson.Status = nameof(LessonStatus.Scheduled);

            _unitOfWork.GetRepository<Lesson, Guid>().Add(lesson);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("End CreateLessonCommandHandler");

            return Result.Success(lesson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateLessonCommandHandler {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
