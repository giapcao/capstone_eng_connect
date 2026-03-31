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
            var courseModules = _unitOfWork.GetRepository<CourseCourseModule, Guid>().FindAll();

            if (query.CourseId.HasValue)
            {
                courseModules = courseModules.Where(x => x.CourseId == query.CourseId.Value);
            }

            var resultQuery = courseModules
                .Select(x => new GetCourseModuleResponse
                {
                    Id = x.CourseModuleId,
                    CourseId = x.CourseId,
                    Title = x.CourseModule.Title,
                    Description = x.CourseModule.Description,
                    Outcomes = x.CourseModule.Outcomes,
                    ModuleNumber = x.ModuleNumber,
                    CreatedAt = x.CourseModule.CreatedAt,
                    UpdatedAt = x.CourseModule.UpdatedAt
                })
                .ApplySearch(query.GetSearchParams(),
                    x => x.Title ?? string.Empty,
                    x => x.Description ?? string.Empty,
                    x => x.Outcomes ?? string.Empty)
                .ApplySorting(query.GetSortParams());

            var result = await resultQuery.ToPaginatedListAsync(query.GetPaginationParams());

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
