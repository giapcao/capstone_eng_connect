using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.CourseSessions.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseSessions.GetListCourseSessionByTutor;

public class GetListCourseSessionByTutorQueryHandler : IQueryHandler<GetListCourseSessionByTutorQuery, PaginationResult<GetCourseSessionResponse>>
{
    private readonly ILogger<GetListCourseSessionByTutorQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListCourseSessionByTutorQueryHandler(ILogger<GetListCourseSessionByTutorQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetCourseSessionResponse>>> HandleAsync(GetListCourseSessionByTutorQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListCourseSessionByTutorQueryHandler {@Query}", query);
        try
        {
            var courseSessions = _unitOfWork.GetRepository<CourseSession, Guid>().FindAll();
            Expression<Func<CourseSession, bool>>? predicate = x => true;

            if (query.TutorId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.TutorId == query.TutorId.Value);
            }

            if (query.CourseModuleId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => !x.CourseModuleCourseSessions.Any(x => x.CourseModuleId == query.CourseModuleId.Value));
            }

            var resultQuery = courseSessions
                .Where(predicate)
                .Select(x => new GetCourseSessionResponse
                {
                    Id = x.Id,
                    ModuleId = query.CourseModuleId ?? Guid.Empty,
                    Title = x.Title,
                    Description = x.Description,
                    Outcomes = x.Outcomes,
                    SessionNumber = null,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ApplySearch(query.GetSearchParams(),
                    x => x.Title ?? string.Empty,
                    x => x.Description ?? string.Empty,
                    x => x.Outcomes ?? string.Empty)
                .ApplySorting(query.GetSortParams());

            var result = await resultQuery.ToPaginatedListAsync(query.GetPaginationParams());

            _logger.LogInformation("End GetListCourseSessionByTutorQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListCourseSessionByTutorQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetCourseSessionResponse>>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
