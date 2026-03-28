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
    private readonly IAwsStorageService _awsStorageService;

    public GetListCourseResourceQueryHandler(ILogger<GetListCourseResourceQueryHandler> logger, IUnitOfWork unitOfWork, IAwsStorageService awsStorageService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _awsStorageService = awsStorageService;
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


            if (query.TutorId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.TutorId == query.TutorId.Value);
            }
            
            if (query.CourseSessionId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => !x.CourseSessionCourseResources.Any(cr => cr.CourseSessionId == query.CourseSessionId.Value));
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
                    x => x.Title ?? string.Empty,
                    x => x.Url)
                .ApplySorting(query.GetSortParams());

            // Map to GetCourseResourceResponse
            var result =
                await courseResources.ProjectToPaginatedListAsync<CourseResource, GetCourseResourceResponse>(
                    query.GetPaginationParams());

            // Convert relative paths to full AWS S3 URLs
            foreach (var item in result.Items)
            {
                item.Url = item.Url != null ? _awsStorageService.GetFileUrl(item.Url) : null!;
            }

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
