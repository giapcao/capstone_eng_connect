using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Courses.UpdateCourse;

public class UpdateCourseCommandHandler : ICommandHandler<UpdateCourseCommand>
{
    private readonly ILogger<UpdateCourseCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseCommandHandler(ILogger<UpdateCourseCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateCourseCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateCourseCommandHandler {@Command}", command);
        try
        {
            var courseRepo = _unitOfWork.GetRepository<Course, Guid>();

            var course = await courseRepo.FindSingleAsync(
                x => x.Id == command.Id,
                cancellationToken: cancellationToken);
            if (course == null)
            {
                _logger.LogWarning("Course not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseNotFound", "Khóa học không tồn tại"));
            }

            //Check tutor
            if (course.TutorId != command.TutorId)
            {
                _logger.LogWarning("Tutor ID mismatch for course ID: {CourseId}", command.Id);
                return Result.Failure(HttpStatusCode.BadRequest, CourseErrors.TutorIsNotOwner());
            }

            course.Title = command.Title;
            course.ShortDescription = command.ShortDescription;
            course.FullDescription = command.FullDescription;
            course.Outcomes = command.Outcomes;
            course.Level = command.Level;
            course.EstimatedTime = TimeSpan.FromMinutes(command.EstimatedTimeLesson * course.NumberOfSessions);
            course.EstimatedTimeLesson = TimeSpan.FromMinutes(command.EstimatedTimeLesson);
            course.Price = command.Price;
            course.Currency = command.Currency;
            course.NumsSessionInWeek = command.NumsSessionInWeek;
            course.ThumbnailUrl = command.ThumbnailUrl;
            course.DemoVideoUrl = command.DemoVideoUrl;
            course.Status = command.Status;
            course.IsCertificate = command.IsCertificate;

            courseRepo.Update(course);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdateCourseCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateCourseCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}