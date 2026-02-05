using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseEnrollments.DeleteCourseEnrollment;

public class DeleteCourseEnrollmentCommandHandler : ICommandHandler<DeleteCourseEnrollmentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteCourseEnrollmentCommandHandler> _logger;

    public DeleteCourseEnrollmentCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteCourseEnrollmentCommandHandler> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeleteCourseEnrollmentCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteCourseEnrollmentCommandHandler: {@command}", command);
        try
        {
            var enrollment = await _unitOfWork.GetRepository<CourseEnrollment, Guid>().FindByIdAsync(command.Id, cancellationToken: cancellationToken);

            if (enrollment == null)
            {
                _logger.LogWarning("CourseEnrollment not found: {id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<CourseEnrollment>("Đăng ký khóa học"));
            }

            _unitOfWork.GetRepository<CourseEnrollment, Guid>().Delete(enrollment);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("End DeleteCourseEnrollmentCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteCourseEnrollmentCommandHandler: {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
