using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Lessons.DeleteLesson;

public class DeleteLessonCommandHandler : ICommandHandler<DeleteLessonCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteLessonCommandHandler> _logger;

    public DeleteLessonCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteLessonCommandHandler> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeleteLessonCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteLessonCommandHandler: {@command}", command);
        try
        {
            var lesson = await _unitOfWork.GetRepository<Lesson, Guid>().FindByIdAsync(
                command.Id,
                true,
                cancellationToken,
                x => x.LessonRecord!,
                x => x.LessonScript!,
                x => x.LessonHomeworks);

            if (lesson == null)
            {
                _logger.LogWarning("Lesson not found: {id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Lesson>("Bài học"));
            }

            if (lesson.LessonScript != null)
            {
                _unitOfWork.GetRepository<LessonScript, Guid>().Delete(lesson.LessonScript);
            }

            if (lesson.LessonRecord != null)
            {
                _unitOfWork.GetRepository<LessonRecord, Guid>().Delete(lesson.LessonRecord);
            }

            foreach (var homework in lesson.LessonHomeworks)
            {
                _unitOfWork.GetRepository<LessonHomework, Guid>().Delete(homework);
            }

            _unitOfWork.GetRepository<Lesson, Guid>().Delete(lesson);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("End DeleteLessonCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteLessonCommandHandler: {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
