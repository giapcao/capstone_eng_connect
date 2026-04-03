using System.Net;
using EngConnect.Application.UseCases.CourseReviews.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseReviews.CreateCourseReview;

public class CreateCourseReviewCommandHandler : ICommandHandler<CreateCourseReviewCommand>
{
    private readonly ILogger<CreateCourseReviewCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseReviewCommandHandler(ILogger<CreateCourseReviewCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateCourseReviewCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateCourseReviewCommandHandler {@Command}", command);
        try
        {
            var courseReview = command.Adapt<CourseReview>();
            var courseReviewRepo = _unitOfWork.GetRepository<CourseReview, Guid>();
            var enrollmentRepo = _unitOfWork.GetRepository<CourseEnrollment, Guid>();
            var courseRepo = _unitOfWork.GetRepository<Course, Guid>();
            
            var enrollment = await enrollmentRepo.FindFirstAsync(
                x => x.Id == command.EnrollmentId && x.StudentId == command.StudentId && x.CourseId == command.CourseId,
                cancellationToken: cancellationToken);
            if (enrollment == null)
            {
                _logger.LogWarning("Enrollment not found or does not match student and course: EnrollmentId={EnrollmentId}, StudentId={StudentId}, CourseId={CourseId}",
                    command.EnrollmentId, command.StudentId, command.CourseId);
                return Result.Failure<GetCourseReviewResponse>(HttpStatusCode.NotFound, new Error("EnrollmentNotFound", "Enrollment không tồn tại hoặc không khớp"));
            }
            
            var course = await courseRepo.FindFirstAsync(x => x.Id == command.CourseId, cancellationToken: cancellationToken);
            if (course == null)
            {
                _logger.LogWarning("Course not found with ID: {CourseId}", command.CourseId);
                return Result.Failure<GetCourseReviewResponse>(HttpStatusCode.NotFound, new Error("CourseNotFound", "Khóa học không tồn tại"));
            }
            
            var existingReview = await courseReviewRepo.AnyAsync(x => x.EnrollmentId == command.EnrollmentId, cancellationToken);
            if (existingReview)
            {
                _logger.LogWarning("Review already exists for enrollment: {EnrollmentId}", command.EnrollmentId);
                return Result.Failure<GetCourseReviewResponse>(HttpStatusCode.Conflict, new Error("ReviewAlreadyExists", "Đánh giá đã tồn tại cho enrollment này"));
            }
            
            courseReviewRepo.Add(courseReview);
            await _unitOfWork.SaveChangesAsync();
            var response = courseReview.Adapt<GetCourseReviewResponse>();
            _logger.LogInformation("End CreateCourseReviewCommandHandler successfully");
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateCourseReviewCommandHandler: {Message}", ex.Message);
            return Result.Failure<GetCourseReviewResponse>(HttpStatusCode.InternalServerError, new Error("InternalError", "Lỗi hệ thống"));
        }
    }
}