using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.CourseSessionCourseResources.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseSessionCourseResources.GetListCourseSessionCourseResource;

public class GetListCourseSessionCourseResourceQueryHandler : IQueryHandler<GetListCourseSessionCourseResourceQuery, PaginationResult<GetCourseSessionCourseResourceResponse>>
{
    private readonly ILogger<GetListCourseSessionCourseResourceQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListCourseSessionCourseResourceQueryHandler(ILogger<GetListCourseSessionCourseResourceQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetCourseSessionCourseResourceResponse>>> HandleAsync(GetListCourseSessionCourseResourceQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListCourseSessionCourseResourceQueryHandler {@Query}", query);
        try
        {
            var courseSessionCourseResources = _unitOfWork.GetRepository<CourseSessionCourseResource, Guid>()
                .FindAll();

            Expression<Func<CourseSessionCourseResource, bool>>? predicate = x => true;

            // Apply filters
            if (query.CourseSessionId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.CourseSessionId == query.CourseSessionId.Value);
            }

            if (query.CourseResourceId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.CourseResourceId == query.CourseResourceId.Value);
            }

            courseSessionCourseResources = courseSessionCourseResources.Where(predicate);

            // Apply sorting
            courseSessionCourseResources = courseSessionCourseResources.ApplySorting(query.GetSortParams());
            

            var result = await courseSessionCourseResources.ProjectToPaginatedListAsync<CourseSessionCourseResource, GetCourseSessionCourseResourceResponse>(query.GetPaginationParams());

            _logger.LogInformation("End GetListCourseSessionCourseResourceQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListCourseSessionCourseResourceQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetCourseSessionCourseResourceResponse>>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
