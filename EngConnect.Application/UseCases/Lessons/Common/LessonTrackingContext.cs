using System.Net;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;

namespace EngConnect.Application.UseCases.Lessons.Common;

internal sealed class LessonTrackingContext



    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
{





    public required CourseEnrollment Enrollment { get; init; }

    public CourseModule? Module { get; init; }

    public CourseSession? Session { get; init; }

    public Guid TutorId =>
        Session?.TutorId
        ?? Module?.TutorId
        ?? Enrollment.Course.TutorId;

    public static async Task<Result<LessonTrackingContext>> ResolveAsync(
        IUnitOfWork unitOfWork,
        Guid enrollmentId,
        Guid studentId,
        Guid? moduleId,
        Guid? sessionId,
        CancellationToken cancellationToken)
    {
        if (moduleId.HasValue != sessionId.HasValue)
        {
            return Result.Failure<LessonTrackingContext>(
                HttpStatusCode.BadRequest,
                new Error("Lesson.Tracking.InvalidShape", "ModuleId và SessionId phải cùng có giá trị hoặc cùng để trống."));
        }

        var enrollment = await unitOfWork.GetRepository<CourseEnrollment, Guid>()
            .FindByIdAsync(
                enrollmentId,
                tracking: false,
                cancellationToken: cancellationToken,
                x => x.Course);

        if (enrollment is null)
        {
            return Result.Failure<LessonTrackingContext>(
                HttpStatusCode.BadRequest,
                CommonErrors.NotFound<CourseEnrollment>("EnrollmentId"));
        }

        if (enrollment.StudentId != studentId)
        {
            return Result.Failure<LessonTrackingContext>(
                HttpStatusCode.BadRequest,
                new Error("Lesson.Tracking.StudentMismatch", "StudentId không thuộc EnrollmentId đã chọn."));
        }

        if (!moduleId.HasValue || !sessionId.HasValue)
        {
            return Result.Success(new LessonTrackingContext
            {
                Enrollment = enrollment
            });
        }

        var module = await unitOfWork.GetRepository<CourseModule, Guid>()
            .FindByIdAsync(moduleId.Value, tracking: false, cancellationToken: cancellationToken);

        if (module is null)
        {
            return Result.Failure<LessonTrackingContext>(
                HttpStatusCode.BadRequest,
                CommonErrors.NotFound<CourseModule>("ModuleId"));
        }

        var session = await unitOfWork.GetRepository<CourseSession, Guid>()
            .FindByIdAsync(sessionId.Value, tracking: false, cancellationToken: cancellationToken);

        if (session is null)
        {
            return Result.Failure<LessonTrackingContext>(
                HttpStatusCode.BadRequest,
                CommonErrors.NotFound<CourseSession>("Buổi học (Session)"));
        }

        var moduleBelongsToEnrollmentCourse = await unitOfWork.GetRepository<CourseCourseModule, Guid>()
            .AnyAsync(
                x => x.CourseId == enrollment.CourseId && x.CourseModuleId == moduleId.Value,
                cancellationToken: cancellationToken);

        if (!moduleBelongsToEnrollmentCourse)
        {
            return Result.Failure<LessonTrackingContext>(
                HttpStatusCode.BadRequest,
                new Error("Lesson.Tracking.ModuleCourseMismatch", "ModuleId không thuộc course của enrollment."));
        }

        var sessionBelongsToModule = await unitOfWork.GetRepository<CourseModuleCourseSession, Guid>()
            .AnyAsync(
                x => x.CourseModuleId == moduleId.Value && x.CourseSessionId == sessionId.Value,
                cancellationToken: cancellationToken);

        if (!sessionBelongsToModule)
        {
            return Result.Failure<LessonTrackingContext>(
                HttpStatusCode.BadRequest,
                new Error("Lesson.Tracking.SessionModuleMismatch", "SessionId không thuộc ModuleId đã chọn."));
        }

        return Result.Success(new LessonTrackingContext
        {
            Enrollment = enrollment,
            Module = module,
            Session = session
        });
    }
}
