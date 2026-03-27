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
            var courseSessions = _unitOfWork.GetRepository<CourseSession, Guid>()
                .FindAll();

            Expression<Func<CourseSession, bool>>? predicate = x => true;
            
            if (query.TutorId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.TutorId == query.TutorId.Value);
            }
            
            if (query.CourseModuleId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.CourseModuleCourseSessions.Any(cmcs => cmcs.CourseModuleId == query.CourseModuleId.Value));
            }

            courseSessions = courseSessions.Where(predicate);

            // Apply search and sort
            courseSessions = courseSessions.ApplySearch(query.GetSearchParams(),
                    x => x.Title ?? string.Empty,
                    x => x.Description ?? string.Empty,
                    x => x.Outcomes ?? string.Empty)
                .ApplySorting(query.GetSortParams());

            // Map to GetCourseSessionResponse
            var result =
                await courseSessions.ProjectToPaginatedListAsync<CourseSession, GetCourseSessionResponse>(
                    query.GetPaginationParams());

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
