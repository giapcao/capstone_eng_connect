using System.Net;
using EngConnect.Application.UseCases.Courses.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Courses.UpdateCourse;

public class UpdateCourseCommandHandler : ICommandHandler<UpdateCourseCommand, GetCourseResponse>
{
    private readonly ILogger<UpdateCourseCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseCommandHandler(ILogger<UpdateCourseCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetCourseResponse>> HandleAsync(UpdateCourseCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateCourseCommandHandler {@Command}", command);
        try
        {
            var courseRepo = _unitOfWork.GetRepository<Course, Guid>();

            var course = await courseRepo.FindSingleAsync(
                x => x.Id == command.Id,
                cancellationToken: cancellationToken);
            if (ValidationUtil.IsNullOrEmpty(course))
            {
                _logger.LogWarning("Course not found with ID: {Id}", command.Id);
                return Result.Failure<GetCourseResponse>(HttpStatusCode.NotFound, new Error("CourseNotFound", "Khóa học không tồn tại"));
            }
            
            // Check status of course
            if (course.Status != nameof(CourseStatus.InActive))
            {
                _logger.LogWarning("Course with ID: {CourseId} cannot be updated", command.Id);
                return Result.Failure<GetCourseResponse>(HttpStatusCode.BadRequest, CourseErrors.PublishedCourseCannotBeUpdated());
            }

            //Check tutor
            if (course.TutorId != command.TutorId)
            {
                _logger.LogWarning("Tutor ID mismatch for course ID: {CourseId}", command.Id);
                return Result.Failure<GetCourseResponse>(HttpStatusCode.BadRequest, CourseErrors.TutorIsNotOwner());
            }

            course.Title = command.Title ?? course.Title;
            course.ShortDescription = command.ShortDescription ?? course.ShortDescription;
            course.FullDescription = command.FullDescription ?? course.FullDescription;
            course.Outcomes = command.Outcomes ?? course.Outcomes;
            course.Level = command.Level ?? course.Level;
            if (command.EstimatedTimeLesson.HasValue)
            {
                course.EstimatedTime = TimeSpan.FromMinutes(
                    command.EstimatedTimeLesson.Value * course.NumberOfSessions
                );
            }
            course.EstimatedTimeLesson = command.EstimatedTimeLesson.HasValue
                ? TimeSpan.FromMinutes(command.EstimatedTimeLesson.Value)
                : course.EstimatedTimeLesson;            
            course.Price = command.Price ?? course.Price;
            course.Currency = command.Currency ?? course.Currency;
            course.NumsSessionInWeek = command.NumsSessionInWeek ?? course.NumsSessionInWeek;
            
            // If course is updated, set status to draft and require tutor to submit for verification again
            course.Status = nameof(CourseStatus.Draft);
            course.IsCertificate = command.IsCertificate ?? course.IsCertificate;

            courseRepo.Update(course);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdateCourseCommandHandler");
            return Result.Success(new GetCourseResponse
            {
                Id = course.Id,
                TutorId = course.TutorId,
                ParentCourseId = course.ParentCourseId,
                Title = course.Title,
                ShortDescription = course.ShortDescription,
                FullDescription = course.FullDescription,
                Outcomes = course.Outcomes,
                Level = course.Level,
                EstimatedTime = course.EstimatedTime,
                EstimatedTimeLesson = course.EstimatedTimeLesson,
                Price = course.Price,
                Currency = course.Currency,
                NumberOfSessions = course.NumberOfSessions,
                NumsSessionInWeek = course.NumsSessionInWeek,
                ThumbnailUrl = course.ThumbnailUrl,
                DemoVideoUrl = course.DemoVideoUrl,
                Status = course.Status,
                IsCertificate = course.IsCertificate,
                NumberOfEnrollment = course.NumberOfEnrollment,
                RatingAverage = course.RatingAverage,
                RatingCount = course.RatingCount,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateCourseCommandHandler: {Message}", ex.Message);
            return Result.Failure<GetCourseResponse>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}