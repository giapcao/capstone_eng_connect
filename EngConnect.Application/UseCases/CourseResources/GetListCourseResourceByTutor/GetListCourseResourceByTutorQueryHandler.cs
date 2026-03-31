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

namespace EngConnect.Application.UseCases.CourseResources.GetListCourseResourceByTutor;

public class GetListCourseResourceByTutorQueryHandler : IQueryHandler<GetListCourseResourceByTutorQuery, PaginationResult<GetCourseResourceResponse>>
{
    private readonly ILogger<GetListCourseResourceByTutorQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAwsStorageService _awsStorageService;

    public GetListCourseResourceByTutorQueryHandler(ILogger<GetListCourseResourceByTutorQueryHandler> logger, IUnitOfWork unitOfWork, IAwsStorageService awsStorageService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _awsStorageService = awsStorageService;
    }

    public async Task<Result<PaginationResult<GetCourseResourceResponse>>> HandleAsync(GetListCourseResourceByTutorQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListCourseResourceByTutorQueryHandler {@Query}", query);
        try
        {
            var courseResources = _unitOfWork.GetRepository<CourseResource, Guid>().FindAll();
            Expression<Func<CourseResource, bool>>? predicate = x => true;

            if (query.TutorId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.TutorId == query.TutorId.Value);
            }

            if (query.CourseSessionId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => !x.CourseSessionCourseResources.Any(x => x.CourseSessionId == query.CourseSessionId.Value));
            }

            if (ValidationUtil.IsNotNullOrEmpty(query.ResourceType))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.ResourceType == query.ResourceType);
            }

            if (ValidationUtil.IsNotNullOrEmpty(query.Status))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Status == query.Status);
            }

            var resultQuery = courseResources
                .Where(predicate)
                .Select(x => new GetCourseResourceResponse
                {
                    Id = x.Id,
                    TutorId = x.TutorId ?? Guid.Empty,
                    Title = x.Title,
                    ResourceType = x.ResourceType,
                    Url = x.Url,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
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

            _logger.LogInformation("End GetListCourseResourceByTutorQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListCourseResourceByTutorQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetCourseResourceResponse>>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
