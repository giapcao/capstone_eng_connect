using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.CourseModuleCourseSessions.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseModuleCourseSessions.GetListCourseModuleCourseSession;

public class GetListCourseModuleCourseSessionQueryHandler : IQueryHandler<GetListCourseModuleCourseSessionQuery, PaginationResult<GetCourseModuleCourseSessionResponse>>
{
    private readonly ILogger<GetListCourseModuleCourseSessionQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListCourseModuleCourseSessionQueryHandler(ILogger<GetListCourseModuleCourseSessionQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetCourseModuleCourseSessionResponse>>> HandleAsync(GetListCourseModuleCourseSessionQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListCourseModuleCourseSessionQueryHandler {@Query}", query);
        try
        {
            var courseModuleCourseSessions = _unitOfWork.GetRepository<CourseModuleCourseSession, Guid>()
                .FindAll();

            Expression<Func<CourseModuleCourseSession, bool>>? predicate = x => true;

            // Apply filters
            if (query.CourseModuleId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.CourseModuleId == query.CourseModuleId.Value);
            }

            if (query.CourseSessionId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.CourseSessionId == query.CourseSessionId.Value);
            }

            courseModuleCourseSessions = courseModuleCourseSessions.Where(predicate);

            // Apply sorting
            courseModuleCourseSessions = courseModuleCourseSessions.ApplySorting(
                query.GetSortParams());
            

            var result = await courseModuleCourseSessions.ProjectToPaginatedListAsync<CourseModuleCourseSession, GetCourseModuleCourseSessionResponse>(query.GetPaginationParams());

            _logger.LogInformation("End GetListCourseModuleCourseSessionQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListCourseModuleCourseSessionQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetCourseModuleCourseSessionResponse>>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
