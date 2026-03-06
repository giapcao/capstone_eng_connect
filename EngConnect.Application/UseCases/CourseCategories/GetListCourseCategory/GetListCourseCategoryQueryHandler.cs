using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.CourseCategories.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseCategories.GetListCourseCategory;

public class GetListCourseCategoryQueryHandler : IQueryHandler<GetListCourseCategoryQuery, PaginationResult<GetCourseCategoryResponse>>
{
    private readonly ILogger<GetListCourseCategoryQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListCourseCategoryQueryHandler(ILogger<GetListCourseCategoryQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetCourseCategoryResponse>>> HandleAsync(GetListCourseCategoryQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListCourseCategoryQueryHandler {@Query}", query);
        try
        {
            var courseCategories = _unitOfWork.GetRepository<CourseCategory, Guid>()
                .FindAll();

            Expression<Func<CourseCategory, bool>>? predicate = x => true;

            // Apply filters
            if (query.CourseId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.CourseId == query.CourseId.Value);
            }

            if (query.CategoryId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.CategoryId == query.CategoryId.Value);
            }

            courseCategories = courseCategories.Where(predicate);

            // Apply search and sort
            courseCategories = courseCategories.ApplySearch(query.GetSearchParams(),
                    x => x.CourseId.ToString(),
                    x => x.CategoryId.ToString())
                .ApplySorting(query.GetSortParams());

            // Map to GetCourseCategoryResponse
            var result =
                await courseCategories.ProjectToPaginatedListAsync<CourseCategory, GetCourseCategoryResponse>(
                    query.GetPaginationParams());

            _logger.LogInformation("End GetListCourseCategoryQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListCourseCategoryQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetCourseCategoryResponse>>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
