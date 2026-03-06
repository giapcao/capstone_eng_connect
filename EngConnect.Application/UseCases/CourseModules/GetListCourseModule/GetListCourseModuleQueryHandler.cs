using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.CourseModules.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseModules.GetListCourseModule;

public class GetListCourseModuleQueryHandler : IQueryHandler<GetListCourseModuleQuery, PaginationResult<GetCourseModuleResponse>>
{
    private readonly ILogger<GetListCourseModuleQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListCourseModuleQueryHandler(ILogger<GetListCourseModuleQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetCourseModuleResponse>>> HandleAsync(GetListCourseModuleQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListCourseModuleQueryHandler {@Query}", query);
        try
        {
            var courseModules = _unitOfWork.GetRepository<CourseModule, Guid>()
                .FindAll();

            Expression<Func<CourseModule, bool>>? predicate = x => true;

            // Apply filters
            if (query.CourseId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.CourseId == query.CourseId.Value);
            }

            courseModules = courseModules.Where(predicate);

            // Apply search and sort
            courseModules = courseModules.ApplySearch(query.GetSearchParams(),
                    x => x.Title,
                    x => x.Description)
                .ApplySorting(query.GetSortParams());

            // Map to GetCourseModuleResponse
            var result =
                await courseModules.ProjectToPaginatedListAsync<CourseModule, GetCourseModuleResponse>(
                    query.GetPaginationParams());

            _logger.LogInformation("End GetListCourseModuleQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListCourseModuleQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetCourseModuleResponse>>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
