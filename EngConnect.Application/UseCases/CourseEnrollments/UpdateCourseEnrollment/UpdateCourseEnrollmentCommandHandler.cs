using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseEnrollments.UpdateCourseEnrollment;

public class UpdateCourseEnrollmentCommandHandler : ICommandHandler<UpdateCourseEnrollmentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCourseEnrollmentCommandHandler> _logger;

    public UpdateCourseEnrollmentCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateCourseEnrollmentCommandHandler> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateCourseEnrollmentCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateCourseEnrollmentCommandHandler: {@command}", command);
        try
        {
            var enrollment = await _unitOfWork.GetRepository<CourseEnrollment, Guid>()
                .FindFirstAsync(x=>
                    x.Id == command.Id 
                    && x.CourseId == command.CourseId
                    && x.StudentId == command.StudentId, cancellationToken: cancellationToken);

            if (enrollment == null)
            {
                _logger.LogWarning("CourseEnrollment not found: {id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<CourseEnrollment>("CourseEnrollment"));
            }

            command.Adapt(enrollment);
            
            _unitOfWork.GetRepository<CourseEnrollment, Guid>().Update(enrollment);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("End UpdateCourseEnrollmentCommandHandler");
            return Result.Success(enrollment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateCourseEnrollmentCommandHandler: {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
