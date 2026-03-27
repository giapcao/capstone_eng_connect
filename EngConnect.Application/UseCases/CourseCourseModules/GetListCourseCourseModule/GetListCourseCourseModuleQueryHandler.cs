using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.CourseCourseModules.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseCourseModules.GetListCourseCourseModule;

public class GetListCourseCourseModuleQueryHandler : IQueryHandler<GetListCourseCourseModuleQuery, PaginationResult<GetCourseCourseModuleResponse>>
{
    private readonly ILogger<GetListCourseCourseModuleQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListCourseCourseModuleQueryHandler(ILogger<GetListCourseCourseModuleQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetCourseCourseModuleResponse>>> HandleAsync(GetListCourseCourseModuleQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListCourseCourseModuleQueryHandler {@Query}", query);
        try
        {
            var courseCourseModules = _unitOfWork.GetRepository<CourseCourseModule, Guid>()
                .FindAll();

            Expression<Func<CourseCourseModule, bool>>? predicate = x => true;

            // Apply filters
            if (query.CourseId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.CourseId == query.CourseId.Value);
            }

            if (query.CourseModuleId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.CourseModuleId == query.CourseModuleId.Value);
            }

            courseCourseModules = courseCourseModules.Where(predicate);

            // Apply sorting
            courseCourseModules = courseCourseModules.ApplySorting(query.GetSortParams());
            

            var result = await courseCourseModules.ProjectToPaginatedListAsync<CourseCourseModule, GetCourseCourseModuleResponse>(query.GetPaginationParams());

            _logger.LogInformation("End GetListCourseCourseModuleQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListCourseCourseModuleQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetCourseCourseModuleResponse>>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
