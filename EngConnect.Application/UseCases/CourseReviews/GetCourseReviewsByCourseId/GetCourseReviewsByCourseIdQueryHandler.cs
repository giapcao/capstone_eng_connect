using System.Net;
using EngConnect.Application.UseCases.CourseReviews.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseReviews.GetCourseReviewsByCourseId;

public class GetCourseReviewsByCourseIdQueryHandler : IQueryHandler<GetCourseReviewsByCourseIdQuery, List<GetCourseReviewResponse>>
{
    private readonly ILogger<GetCourseReviewsByCourseIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetCourseReviewsByCourseIdQueryHandler(ILogger<GetCourseReviewsByCourseIdQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<GetCourseReviewResponse>>> HandleAsync(GetCourseReviewsByCourseIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetCourseReviewsByCourseIdQueryHandler {@Query}", query);
        try
        {
            var courseReviewRepo = _unitOfWork.GetRepository<CourseReview, Guid>();

            var reviews = courseReviewRepo.FindAll(x => x.CourseId == query.CourseId)
                .OrderByDescending(x => x.CreatedAt)
                .Skip((query.Page.GetValueOrDefault(1) - 1) * query.PageSize.GetValueOrDefault(10))
                .Take(query.PageSize.GetValueOrDefault(10))
                .ToList();

            var responses = reviews.Select(review => new GetCourseReviewResponse
            {
                Id = review.Id,
                CourseId = review.CourseId,
                TutorId = review.TutorId,
                StudentId = review.StudentId,
                EnrollmentId = review.EnrollmentId,
                Rating = review.Rating,
                Comment = review.Comment,
                IsAnonymous = review.IsAnonymous,
                CreatedAt = review.CreatedAt,
                UpdatedAt = review.UpdatedAt
            }).ToList();

            _logger.LogInformation("End GetCourseReviewsByCourseIdQueryHandler successfully");
            return Result.Success(responses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetCourseReviewsByCourseIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<List<GetCourseReviewResponse>>(HttpStatusCode.InternalServerError, new Error("InternalError", "Lỗi hệ thống"));
        }
    }
}