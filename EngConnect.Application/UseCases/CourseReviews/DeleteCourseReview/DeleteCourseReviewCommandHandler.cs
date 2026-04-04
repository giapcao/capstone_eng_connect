using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseReviews.DeleteCourseReview;

public class DeleteCourseReviewCommandHandler : ICommandHandler<DeleteCourseReviewCommand>
{
    private readonly ILogger<DeleteCourseReviewCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourseReviewCommandHandler(ILogger<DeleteCourseReviewCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeleteCourseReviewCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteCourseReviewCommandHandler {@Command}", command);
        try
        {
            var courseReviewRepo = _unitOfWork.GetRepository<CourseReview, Guid>();

            var courseReview = await courseReviewRepo.FindFirstAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);
            if (courseReview == null)
            {
                _logger.LogWarning("CourseReview not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseReviewNotFound", "Đánh giá khóa học không tồn tại"));
            }

            courseReviewRepo.Delete(courseReview);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End DeleteCourseReviewCommandHandler successfully");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteCourseReviewCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, new Error("InternalError", "Lỗi hệ thống"));
        }
    }
}