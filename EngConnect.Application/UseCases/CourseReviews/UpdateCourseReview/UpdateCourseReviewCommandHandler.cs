using System.Net;
using EngConnect.Application.UseCases.CourseReviews.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseReviews.UpdateCourseReview;

public class UpdateCourseReviewCommandHandler : ICommandHandler<UpdateCourseReviewCommand>
{
    private readonly ILogger<UpdateCourseReviewCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseReviewCommandHandler(ILogger<UpdateCourseReviewCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateCourseReviewCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateCourseReviewCommandHandler {@Command}", command);
        try
        {
            var courseReviewRepo = _unitOfWork.GetRepository<CourseReview, Guid>();

            var courseReviewExist = await courseReviewRepo.FindFirstAsync(x => x.Id == command.Id, cancellationToken: cancellationToken);
            if (courseReviewExist == null)
            {
                _logger.LogWarning("CourseReview not found with ID: {Id}", command.Id);
                return Result.Failure<GetCourseReviewResponse>(HttpStatusCode.NotFound, new Error("CourseReviewNotFound", "Đánh giá khóa học không tồn tại"));
            }
            command.Adapt(courseReviewExist);
            courseReviewRepo.Update(courseReviewExist);
            await _unitOfWork.SaveChangesAsync();
            var response = courseReviewExist.Adapt<GetCourseReviewResponse>();
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