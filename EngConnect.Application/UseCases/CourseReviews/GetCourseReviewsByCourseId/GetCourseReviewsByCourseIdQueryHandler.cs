using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.CourseReviews.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseReviews.GetCourseReviewsByCourseId;

public class GetCourseReviewsByCourseIdQueryHandler : IQueryHandler<GetCourseReviewsByCourseIdQuery, PaginationResult<GetCourseReviewResponse>>
{
    private readonly ILogger<GetCourseReviewsByCourseIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetCourseReviewsByCourseIdQueryHandler(ILogger<GetCourseReviewsByCourseIdQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetCourseReviewResponse>>> HandleAsync(GetCourseReviewsByCourseIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetCourseReviewsByCourseIdQueryHandler {@Query}", query);
        try
        {
            var courseReviewRepository = _unitOfWork.GetRepository<CourseReview, Guid>();
            
            var reviews = courseReviewRepository.FindAll();
            
            Expression<Func<CourseReview,bool>> predicate = x => true;
            if (ValidationUtil.IsNotNullOrEmpty(query.CourseId))
            { 
                predicate = predicate.CombineAndAlsoExpressions(x=>x.CourseId == query.CourseId);
            }
            
            reviews = reviews.Where(predicate);
            
            reviews = reviews.ApplySorting(query.GetSortParams());

            var result =
                await reviews.ProjectToPaginatedListAsync<CourseReview, GetCourseReviewResponse>
                    (query.GetPaginationParams());
            
            _logger.LogInformation("End GetCourseReviewsByCourseIdQueryHandler successfully");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetCourseReviewsByCourseIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetCourseReviewResponse>>(HttpStatusCode.InternalServerError, new Error("InternalError", "Lỗi hệ thống"));
        }
    }
}