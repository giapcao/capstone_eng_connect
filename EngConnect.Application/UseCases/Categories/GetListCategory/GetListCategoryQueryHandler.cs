using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.Categories.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Categories.GetListCategory;

public class GetListCategoryQueryHandler : IQueryHandler<GetListCategoryQuery, PaginationResult<GetCategoryResponse>>
{
    private readonly ILogger<GetListCategoryQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListCategoryQueryHandler(ILogger<GetListCategoryQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetCategoryResponse>>> HandleAsync(GetListCategoryQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListCategoryQueryHandler {@Query}", query);
        try
        {
            var categories = _unitOfWork.GetRepository<Category, Guid>()
                .FindAll();

            Expression<Func<Category, bool>>? predicate = x => true;

            // Apply filters
            if (!string.IsNullOrWhiteSpace(query.Type))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Type == query.Type);
            }

            categories = categories.Where(predicate);

            // Apply search and sort
            categories = categories.ApplySearch(query.GetSearchParams(),
                    x => x.Name,
                    x => x.Description ?? string.Empty,
                    x => x.Type ?? string.Empty)
                .ApplySorting(query.GetSortParams());

            // Map to GetCategoryResponse
            var result =
                await categories.ProjectToPaginatedListAsync<Category, GetCategoryResponse>(
                    query.GetPaginationParams());

            _logger.LogInformation("End GetListCategoryQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListCategoryQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetCategoryResponse>>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
