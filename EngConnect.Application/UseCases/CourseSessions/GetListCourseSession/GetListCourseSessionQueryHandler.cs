using System.Net;
using EngConnect.Application.UseCases.CourseSessions.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseSessions.GetListCourseSession;

public class GetListCourseSessionQueryHandler : IQueryHandler<GetListCourseSessionQuery, PaginationResult<GetCourseSessionResponse>>
{
    private readonly ILogger<GetListCourseSessionQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListCourseSessionQueryHandler(ILogger<GetListCourseSessionQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetCourseSessionResponse>>> HandleAsync(GetListCourseSessionQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListCourseSessionQueryHandler {@Query}", query);
        try
        {
            var courseSessions = _unitOfWork.GetRepository<CourseModuleCourseSession, Guid>().FindAll();

            if (query.CourseModuleId.HasValue)
            {
                courseSessions = courseSessions.Where(x => x.CourseModuleId == query.CourseModuleId.Value);
            }

            var resultQuery = courseSessions
                .Select(x => new GetCourseSessionResponse
                {
                    Id = x.CourseSessionId,
                    ModuleId = x.CourseModuleId,
                    ParentSessionId = x.CourseSession.ParentSessionId,
                    Title = x.CourseSession.Title,
                    Description = x.CourseSession.Description,
                    Outcomes = x.CourseSession.Outcomes,
                    SessionNumber = x.SessionNumber,
                    CreatedAt = x.CourseSession.CreatedAt,
                    UpdatedAt = x.CourseSession.UpdatedAt
                })
                .ApplySearch(query.GetSearchParams(),
                    x => x.Title ?? string.Empty,
                    x => x.Description ?? string.Empty,
                    x => x.Outcomes ?? string.Empty)
                .ApplySorting(query.GetSortParams());

            var result = await resultQuery.ToPaginatedListAsync(query.GetPaginationParams());

            _logger.LogInformation("End GetListCourseSessionQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListCourseSessionQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetCourseSessionResponse>>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
