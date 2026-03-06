using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.Courses.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Courses.GetListCourse;

public class GetListCourseQueryHandler : IQueryHandler<GetListCourseQuery, PaginationResult<GetCourseResponse>>
{
    private readonly ILogger<GetListCourseQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListCourseQueryHandler(ILogger<GetListCourseQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetCourseResponse>>> HandleAsync(GetListCourseQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListCourseQueryHandler {@Query}", query);
        try
        {
            var courses = _unitOfWork.GetRepository<Course, Guid>()
                .FindAll();

            Expression<Func<Course, bool>>? predicate = x => true;

            // Apply filters
            if (query.TutorId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.TutorId == query.TutorId.Value);
            }

            if (ValidationUtil.IsNotNullOrEmpty(query.Level))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Level == query.Level);
            }

            if (ValidationUtil.IsNotNullOrEmpty(query.Status))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Status == query.Status);
            }

            courses = courses.Where(predicate);

            // Apply search and sort
            courses = courses.ApplySearch(query.GetSearchParams(),
                    x => x.Title,
                    x => x.ShortDescription,
                    x => x.FullDescription)
                .ApplySorting(query.GetSortParams());

            // Map to GetCourseResponse
            var result =
                await courses.ProjectToPaginatedListAsync<Course, GetCourseResponse>(
                    query.GetPaginationParams());

            _logger.LogInformation("End GetListCourseQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListCourseQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetCourseResponse>>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
