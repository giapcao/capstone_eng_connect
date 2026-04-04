using System.Net;
using EngConnect.Application.UseCases.Lessons.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Lessons.UpdateLesson;

public class UpdateLessonCommandHandler : ICommandHandler<UpdateLessonCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateLessonCommandHandler> _logger;

    public UpdateLessonCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateLessonCommandHandler> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateLessonCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateLessonCommandHandler: {@command}", command);
        try
        {
            var lesson = await _unitOfWork.GetRepository<Lesson, Guid>()
                .FindByIdAsync(command.Id, cancellationToken: cancellationToken);

            if (lesson == null)
            {
                _logger.LogWarning("Lesson not found: {id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Lesson>("BÃ i há»c"));
            }

            command.Adapt(lesson);

            var studentExists = await _unitOfWork.GetRepository<Student, Guid>()
                .AnyAsync(x => x.Id == lesson.StudentId, cancellationToken: cancellationToken);

            if (!studentExists)
            {
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Student>("thÃ´ng tin Há»c sinh."));
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
                    "Invalid lesson tracking data while updating LessonId {LessonId}. EnrollmentId {EnrollmentId}, ModuleId {ModuleId}, SessionId {SessionId}",
                    command.Id,
                    command.EnrollmentId,
                    command.ModuleId,
                    command.SessionId);
                return Result.Failure(trackingContext.HttpStatusCode, trackingContext.Error!);
            }

            lesson.TutorId = trackingContext.Data!.TutorId;

            _unitOfWork.GetRepository<Lesson, Guid>().Update(lesson);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdateLessonCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateLessonCommandHandler: {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
