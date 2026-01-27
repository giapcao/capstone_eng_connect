using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseEnrollments.CreateCourseEnrollment;

public class CreateCourseEnrollmentCommandHandler : ICommandHandler<CreateCourseEnrollmentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateCourseEnrollmentCommandHandler> _logger;

    public CreateCourseEnrollmentCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateCourseEnrollmentCommandHandler> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> HandleAsync(CreateCourseEnrollmentCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateCourseEnrollmentCommandHandler: {@command}", command);
        try
        {
            var courseEnrollment = command.Adapt<CourseEnrollment>();
            
            var course = await _unitOfWork.GetRepository<Course, Guid>()
                .AnyAsync(x => x.Id == courseEnrollment.CourseId, cancellationToken: cancellationToken);
            
            var student = await _unitOfWork.GetRepository<Student, Guid>()
                .AnyAsync(x => x.Id == courseEnrollment.StudentId, cancellationToken: cancellationToken);
            
            var enrollmentExist = await _unitOfWork.GetRepository<CourseEnrollment, Guid>()
                .AnyAsync(x => x.CourseId == courseEnrollment.CourseId 
                               && x.StudentId == courseEnrollment.StudentId
                    && x.Status == nameof(CourseEnrollmentStatus.InProgress), cancellationToken);
            
            if (!course || !student)
            {
                _logger.LogWarning("CourseId or StudentId does not exist: CourseId={courseId}, StudentId={studentId}", command.CourseId, command.StudentId);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<CourseEnrollment>("Khóa học hoặc học sinh không tồn tại"));
            }

            if (enrollmentExist)
            {
                _logger.LogWarning("CourseEnrollment already exists for CourseId={courseId}, StudentId={studentId}", command.CourseId, command.StudentId);
                return Result.Failure(HttpStatusCode.Conflict, CommonErrors.AlreadyExists("Enrollment", "Học sinh này"));
            }
            
            courseEnrollment.Status = nameof(CourseEnrollmentStatus.InProgress);
            courseEnrollment.Currency = nameof(Currency.Vnd).ToUpper();
            courseEnrollment.EnrolledAt = DateTime.UtcNow;
            _unitOfWork.GetRepository<CourseEnrollment, Guid>().Add(courseEnrollment);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("End CreateCourseEnrollmentCommandHandler");
            
            return Result.Success(courseEnrollment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateCourseEnrollmentCommandHandler {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
