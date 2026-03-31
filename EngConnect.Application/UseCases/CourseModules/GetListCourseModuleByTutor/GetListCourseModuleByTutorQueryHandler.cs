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

namespace EngConnect.Application.UseCases.CourseModules.GetListCourseModuleByTutor;

public class GetListCourseModuleByTutorQueryHandler : IQueryHandler<GetListCourseModuleByTutorQuery, PaginationResult<GetCourseModuleResponse>>
{
    private readonly ILogger<GetListCourseModuleByTutorQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListCourseModuleByTutorQueryHandler(ILogger<GetListCourseModuleByTutorQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetCourseModuleResponse>>> HandleAsync(GetListCourseModuleByTutorQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListCourseModuleByTutorQueryHandler {@Query}", query);
        try
        {
            var courseModules = _unitOfWork.GetRepository<CourseModule, Guid>().FindAll();
            Expression<Func<CourseModule, bool>>? predicate = x => true;

            if (query.TutorId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.TutorId == query.TutorId.Value);
            }

            if (query.CourseId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => !x.CourseCourseModules.Any(x => x.CourseId == query.CourseId.Value));
            }

            var resultQuery = courseModules
                .Where(predicate)
                .Select(x => new GetCourseModuleResponse
                {
                    Id = x.Id,
                    CourseId = query.CourseId ?? Guid.Empty,
                    Title = x.Title,
                    Description = x.Description,
                    Outcomes = x.Outcomes,
                    ModuleNumber = null,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ApplySearch(query.GetSearchParams(),
                    x => x.Title ?? string.Empty,
                    x => x.Description ?? string.Empty,
                    x => x.Outcomes ?? string.Empty)
                .ApplySorting(query.GetSortParams());

            var result = await resultQuery.ToPaginatedListAsync(query.GetPaginationParams());

            _logger.LogInformation("End GetListCourseModuleByTutorQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListCourseModuleByTutorQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetCourseModuleResponse>>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
