using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
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
            
            var enrollment = await _unitOfWork.GetRepository<CourseEnrollment, Guid>()
                .AnyAsync( x=>x.Id == lesson.EnrollmentId, cancellationToken: cancellationToken);
            
            if (!enrollment)
            {
                _logger.LogWarning("EnrollmentId does not exist: {enrollmentId}", command.EnrollmentId);
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.NotFound<CourseEnrollment>("EnrollmentId"));
            }
            
            var studentExists = await _unitOfWork.GetRepository<Student,Guid>()
                .AnyAsync(x=>x.Id == lesson.StudentId, cancellationToken: cancellationToken);
        
            if (!studentExists)
            {
                _logger.LogWarning("Student not found");
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.NotFound<Student>("thông tin Học sinh."));
            }

            if (lesson.SessionId.HasValue)
            {
                var sessionExists = await _unitOfWork.GetRepository<CourseSession, Guid>()
                    .FindFirstAsync(x=>x.Id == lesson.SessionId, cancellationToken: cancellationToken);
            
                if (ValidationUtil.IsNullOrEmpty(sessionExists))
                {
                    _logger.LogWarning("Course session not found with ID: {SessionId}", lesson.SessionId);
                    return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.NotFound<CourseSession>("Buổi học (Session) không tồn tại."));
                }
                
            }
            
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
