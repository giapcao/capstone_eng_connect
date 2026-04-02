using System.Net;
using EngConnect.Application.UseCases.CourseReviews.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseReviews.UpdateCourseReview;

public class UpdateCourseReviewCommandHandler : ICommandHandler<UpdateCourseReviewCommand, GetCourseReviewResponse>
{
    private readonly ILogger<UpdateCourseReviewCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseReviewCommandHandler(ILogger<UpdateCourseReviewCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetCourseReviewResponse>> HandleAsync(UpdateCourseReviewCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateCourseReviewCommandHandler {@Command}", command);
        try
        {
            var courseReviewRepo = _unitOfWork.GetRepository<CourseReview, Guid>();

            var courseReview = await courseReviewRepo.FindFirstAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);
            if (courseReview == null)
            {
                _logger.LogWarning("CourseReview not found with ID: {Id}", command.Id);
                return Result.Failure<GetCourseReviewResponse>(HttpStatusCode.NotFound, new Error("CourseReviewNotFound", "Đánh giá khóa học không tồn tại"));
            }

            // Update fields
            if (command.Rating.HasValue)
                courseReview.Rating = command.Rating;
            if (command.Comment != null)
                courseReview.Comment = command.Comment;
            if (command.IsAnonymous.HasValue)
                courseReview.IsAnonymous = command.IsAnonymous;

            courseReview.UpdatedAt = DateTime.UtcNow;

            courseReviewRepo.Update(courseReview);
            await _unitOfWork.SaveChangesAsync();

            var response = new GetCourseReviewResponse
            {
                Id = courseReview.Id,
                CourseId = courseReview.CourseId,
                TutorId = courseReview.TutorId,
                StudentId = courseReview.StudentId,
                EnrollmentId = courseReview.EnrollmentId,
                Rating = courseReview.Rating,
                Comment = courseReview.Comment,
                IsAnonymous = courseReview.IsAnonymous,
                CreatedAt = courseReview.CreatedAt,
                UpdatedAt = courseReview.UpdatedAt
            };

            _logger.LogInformation("End UpdateCourseReviewCommandHandler successfully");
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateCourseReviewCommandHandler: {Message}", ex.Message);
            return Result.Failure<GetCourseReviewResponse>(HttpStatusCode.InternalServerError, new Error("InternalError", "Lỗi hệ thống"));
        }
    }
}