using System.Net;
using EngConnect.Application.UseCases.CourseEnrollments.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseEnrollments.GetCourseEnrollmentById;

public class GetCourseEnrollmentByIdQueryHandler : IQueryHandler<GetCourseEnrollmentByIdQuery, GetCourseEnrollmentResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetCourseEnrollmentByIdQueryHandler> _logger;

    public GetCourseEnrollmentByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetCourseEnrollmentByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<GetCourseEnrollmentResponse>> HandleAsync(
        GetCourseEnrollmentByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetCourseEnrollmentByIdQueryHandler: {@query}", query);
        try
        {
            var enrollment = await _unitOfWork.GetRepository<CourseEnrollment, Guid>().FindByIdAsync( query.Id, cancellationToken: cancellationToken);

            if (enrollment == null)
            {
                _logger.LogWarning("CourseEnrollment not found: {id}", query.Id);
                return Result.Failure<GetCourseEnrollmentResponse>(
                    HttpStatusCode.NotFound,
                    CommonErrors.NotFound<CourseEnrollment>("Đăng ký khóa học"));
            }

            var enrollmentResponse = enrollment.Adapt<GetCourseEnrollmentResponse>();
            
            _logger.LogInformation("End GetCourseEnrollmentByIdQueryHandler");
            return Result.Success(enrollmentResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetCourseEnrollmentByIdQueryHandler: {@Message}", ex.Message);
            return Result.Failure<GetCourseEnrollmentResponse>(
                HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
