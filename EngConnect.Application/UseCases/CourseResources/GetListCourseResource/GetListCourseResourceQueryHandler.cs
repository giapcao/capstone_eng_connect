using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.CourseResources.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseResources.GetListCourseResource;

public class GetListCourseResourceQueryHandler : IQueryHandler<GetListCourseResourceQuery, PaginationResult<GetCourseResourceResponse>>
{
    private readonly ILogger<GetListCourseResourceQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListCourseResourceQueryHandler(ILogger<GetListCourseResourceQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetCourseResourceResponse>>> HandleAsync(GetListCourseResourceQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListCourseResourceQueryHandler {@Query}", query);
        try
        {
            var courseResources = _unitOfWork.GetRepository<CourseResource, Guid>()
                .FindAll();

            Expression<Func<CourseResource, bool>>? predicate = x => true;

            // Apply filters
            if (query.SessionId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.SessionId == query.SessionId.Value);
            }

            if (ValidationUtil.IsNotNullOrEmpty(query.ResourceType))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.ResourceType == query.ResourceType);
            }

            if (ValidationUtil.IsNotNullOrEmpty(query.Status))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Status == query.Status);
            }

            courseResources = courseResources.Where(predicate);

            // Apply search and sort
            courseResources = courseResources.ApplySearch(query.GetSearchParams(),
                    x => x.Title,
                    x => x.Url)
                .ApplySorting(query.GetSortParams());

            // Map to GetCourseResourceResponse
            var result =
                await courseResources.ProjectToPaginatedListAsync<CourseResource, GetCourseResourceResponse>(
                    query.GetPaginationParams());

            _logger.LogInformation("End GetListCourseResourceQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListCourseResourceQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetCourseResourceResponse>>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
