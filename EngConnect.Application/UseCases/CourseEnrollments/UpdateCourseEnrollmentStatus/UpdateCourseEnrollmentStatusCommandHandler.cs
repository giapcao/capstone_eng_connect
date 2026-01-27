using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseEnrollments.UpdateCourseEnrollmentStatus;

public class UpdateCourseEnrollmentStatusCommandHandler : ICommandHandler<UpdateCourseEnrollmentStatusCommand>
{
    private readonly ILogger<UpdateCourseEnrollmentStatusCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseEnrollmentStatusCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateCourseEnrollmentStatusCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Result> HandleAsync(UpdateCourseEnrollmentStatusCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateCourseEnrollmentStatusCommand: {@command}", command);
        try
        {
            var repository = _unitOfWork.GetRepository<CourseEnrollment, Guid>();
            var enrollment = await repository.FindByIdAsync(command.Id, cancellationToken: cancellationToken);

            if (enrollment == null) 
            {
                _logger.LogWarning("CourseEnrollment not found: {id}", command.Id); 
                return Result.Failure(HttpStatusCode.NotFound, 
                    CommonErrors.NotFound<CourseEnrollment>("Đăng ký khóa học"));
            }

            if (command.NewStatus?.ToLower() == nameof(CourseEnrollmentStatus.Cancelled).ToLower())
            {
                enrollment.Status = nameof(CourseEnrollmentStatus.Cancelled);
                enrollment.ExpiredAt = DateTime.UtcNow; 
            }
            else 
            {
                enrollment.Status = command.NewStatus;
            }

            await _unitOfWork.SaveChangesAsync();
        
            _logger.LogInformation("End UpdateCourseEnrollmentStatusCommand successfully for {Id}", command.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateCourseEnrollmentStatusCommand: {@Message}", ex.Message);
            return Result.Failure(
                HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}