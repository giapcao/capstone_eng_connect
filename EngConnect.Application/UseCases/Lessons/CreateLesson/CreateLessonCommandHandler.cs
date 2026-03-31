using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
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
            var tutorExists = await _unitOfWork.GetRepository<Tutor, Guid>()
                .AnyAsync(x => x.Id == command.TutorId, cancellationToken: cancellationToken);

            if (!tutorExists)
            {
                _logger.LogWarning("Tutor not found: {tutorId}", command.TutorId);
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.NotFound<Tutor>("TutorId"));
            }

            var studentExists = await _unitOfWork.GetRepository<Student, Guid>()
                .AnyAsync(x => x.Id == command.StudentId, cancellationToken: cancellationToken);

            if (!studentExists)
            {
                _logger.LogWarning("Student not found: {studentId}", command.StudentId);
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.NotFound<Student>("StudentId"));
            }

            var enrollment = await _unitOfWork.GetRepository<CourseEnrollment, Guid>()
                .FindByIdAsync(command.EnrollmentId, true, cancellationToken, e => e.Course);

            if (enrollment == null)
            {
                _logger.LogWarning("Enrollment not found: {enrollmentId}", command.EnrollmentId);
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.NotFound<CourseEnrollment>("EnrollmentId"));
            }

            if (enrollment.StudentId != command.StudentId)
            {
                _logger.LogWarning("Enrollment {enrollmentId} does not belong to student {studentId}", command.EnrollmentId, command.StudentId);
                return Result.Failure(HttpStatusCode.BadRequest,
                    CommonErrors.ValidationFailed("Enrollment does not belong to StudentId"));
            }

            if (enrollment.Course.TutorId != command.TutorId)
            {
                _logger.LogWarning("Enrollment {enrollmentId} does not belong to tutor {tutorId}", command.EnrollmentId, command.TutorId);
                return Result.Failure(HttpStatusCode.BadRequest,
                    CommonErrors.ValidationFailed("Enrollment does not belong to TutorId"));
            }

            if (command.SessionId.HasValue)
            {
                var sessionExists = await _unitOfWork.GetRepository<CourseSession, Guid>()
                    .AnyAsync(x => x.Id == command.SessionId, cancellationToken: cancellationToken);

                if (!sessionExists)
                {
                    _logger.LogWarning("Course session not found with ID: {sessionId}", command.SessionId);
                    return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.NotFound<CourseSession>("SessionId"));
                }
            }

            var lesson = new Lesson
            {
                TutorId = command.TutorId,
                StudentId = command.StudentId,
                EnrollmentId = command.EnrollmentId,
                SessionId = command.SessionId,
                StartTime = command.StartTime,
                EndTime = command.EndTime,
                MeetingUrl = command.MeetingUrl,
                Status = nameof(LessonStatus.Scheduled)
            };

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
