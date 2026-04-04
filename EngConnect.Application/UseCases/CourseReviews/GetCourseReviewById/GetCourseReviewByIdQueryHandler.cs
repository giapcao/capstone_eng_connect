using System.Net;
using EngConnect.Application.UseCases.CourseReviews.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseReviews.GetCourseReviewById;

public class GetCourseReviewByIdQueryHandler : IQueryHandler<GetCourseReviewByIdQuery, GetCourseReviewResponse>
{
    private readonly ILogger<GetCourseReviewByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetCourseReviewByIdQueryHandler(ILogger<GetCourseReviewByIdQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetCourseReviewResponse>> HandleAsync(GetCourseReviewByIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetCourseReviewByIdQueryHandler {@Query}", query);
        try
        {
            var courseReviewRepo = _unitOfWork.GetRepository<CourseReview, Guid>();

            var courseReview = await courseReviewRepo.FindFirstAsync(x => x.Id == query.Id,false, cancellationToken: cancellationToken);
            if (courseReview == null)
            {
                _logger.LogWarning("CourseReview not found with ID: {Id}", query.Id);
                return Result.Failure<GetCourseReviewResponse>(HttpStatusCode.NotFound, new Error("CourseReviewNotFound", "Đánh giá khóa học không tồn tại"));
            }
            
            var response = courseReview.Adapt<GetCourseReviewResponse>();

            _logger.LogInformation("End GetCourseReviewByIdQueryHandler successfully");
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetCourseReviewByIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetCourseReviewResponse>(HttpStatusCode.InternalServerError, new Error("InternalError", "Lỗi hệ thống"));
        }
    }
}