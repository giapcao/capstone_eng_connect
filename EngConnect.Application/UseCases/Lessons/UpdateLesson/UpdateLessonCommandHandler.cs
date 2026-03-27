using System.Net;
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
            var lesson = await _unitOfWork.GetRepository<Lesson, Guid>().FindByIdAsync(command.Id, cancellationToken: cancellationToken);

            if (lesson == null)
            {
                _logger.LogWarning("Lesson not found: {id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Lesson>("Bài học"));
            }
            
            command.Adapt(lesson);
            
            var enrollment = await _unitOfWork.GetRepository<CourseEnrollment, Guid>()
                .AnyAsync( x=>x.Id == lesson.EnrollmentId, cancellationToken: cancellationToken);
            
            if (!enrollment)
            {
                _logger.LogWarning("EnrollmentId does not exist: {enrollmentId}", command.EnrollmentId);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<CourseEnrollment>("EnrollmentId"));
            }
            
            var studentExists = await _unitOfWork.GetRepository<Student,Guid>()
                .AnyAsync(x=>x.Id == lesson.StudentId, cancellationToken: cancellationToken);
        
            if (!studentExists)
            {
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Student>("thông tin Học sinh."));
            }

            if (lesson.SessionId.HasValue)
            {
                var sessionExists = await _unitOfWork.GetRepository<CourseSession, Guid>()
                    .AnyAsync(x=>x.Id == lesson.SessionId, cancellationToken: cancellationToken);
            
                if (!sessionExists)
                {
                    return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<CourseSession>("Buổi học (Session)"));
                }
            }
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
