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
            var courseResources = _unitOfWork.GetRepository<CourseSessionCourseResource, Guid>().FindAll();

            if (query.CourseSessionId.HasValue)
            {
                courseResources = courseResources.Where(x => x.CourseSessionId == query.CourseSessionId.Value);
            }

            if (ValidationUtil.IsNotNullOrEmpty(query.ResourceType))
            {
                courseResources = courseResources.Where(x => x.CourseResource.ResourceType == query.ResourceType);
            }

            if (ValidationUtil.IsNotNullOrEmpty(query.Status))
            {
                courseResources = courseResources.Where(x => x.CourseResource.Status == query.Status);
            }

            var resultQuery = courseResources
                .Select(x => new GetCourseResourceResponse
                {
                    Id = x.CourseResourceId,
                    TutorId = x.CourseResource.TutorId ?? Guid.Empty,
                    ParentResourceId = x.CourseResource.ParentResourceId,
                    Title = x.CourseResource.Title,
                    ResourceType = x.CourseResource.ResourceType,
                    Url = x.CourseResource.Url,
                    Status = x.CourseResource.Status,
                    CreatedAt = x.CourseResource.CreatedAt,
                    UpdatedAt = x.CourseResource.UpdatedAt
                })
                .ApplySearch(query.GetSearchParams(),
                    x => x.Title ?? string.Empty,
                    x => x.Url)
                .ApplySorting(query.GetSortParams());

            var result = await resultQuery.ToPaginatedListAsync(query.GetPaginationParams());

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
