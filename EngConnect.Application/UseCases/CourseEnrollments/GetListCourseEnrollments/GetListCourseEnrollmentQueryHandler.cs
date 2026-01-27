using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.CourseEnrollments.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseEnrollments.GetListCourseEnrollments;

public class GetListCourseEnrollmentQueryHandler : IQueryHandler<GetListCourseEnrollmentQuery, PaginationResult<GetCourseEnrollmentResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetListCourseEnrollmentQueryHandler> _logger;

    public GetListCourseEnrollmentQueryHandler(IUnitOfWork unitOfWork, ILogger<GetListCourseEnrollmentQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<PaginationResult<GetCourseEnrollmentResponse>>> HandleAsync(
        GetListCourseEnrollmentQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListCourseEnrollmentQueryHandler: {@query}", query);
        try
        {
            var enrollmentRepository = _unitOfWork.GetRepository<CourseEnrollment, Guid>();

            var enrollments = enrollmentRepository.FindAll();
            
            Expression<Func<CourseEnrollment, bool>> predicate = x => true;
            
            if (ValidationUtil.IsNotNullOrEmpty(query.Status))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Status != null && query.Status.ToLower().Contains(x.Status.ToLower()));
            }
            

            
            if (query.CourseId.HasValue)
                enrollments = enrollments.Where(x => x.CourseId == query.CourseId);

            if (query.StudentId.HasValue)
                predicate = predicate.CombineAndAlsoExpressions(x => x.StudentId == query.StudentId);
            
            if (query.EnrolledFrom.HasValue)
                predicate = predicate.CombineAndAlsoExpressions(x => x.EnrolledAt >= query.EnrolledFrom);
            
            if (query.EnrolledTo.HasValue)
                predicate = predicate.CombineAndAlsoExpressions(x => x.EnrolledAt <= query.EnrolledTo);
            
            if (query.ExpiredFrom.HasValue)
                predicate = predicate.CombineAndAlsoExpressions(x => x.ExpiredAt >= query.ExpiredFrom);

            if (query.ExpiredTo.HasValue)
                predicate = predicate.CombineAndAlsoExpressions(x => x.ExpiredAt <= query.ExpiredTo);
            
            enrollments = enrollments.Where(predicate);
            
            enrollments = enrollments.ApplySorting(query.GetSortParams());

            var result = await enrollments.ProjectToPaginatedListAsync<CourseEnrollment, GetCourseEnrollmentResponse>
                (query.GetPaginationParams());

            _logger.LogInformation("End GetListCourseEnrollmentQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListCourseEnrollmentQueryHandler: {@Message}", ex.Message);
            return Result.Failure<PaginationResult<GetCourseEnrollmentResponse>>(
                HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
