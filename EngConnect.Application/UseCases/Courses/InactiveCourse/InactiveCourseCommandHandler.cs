using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Courses.InactiveCourse;

public class InactiveCourseCommandHandler : ICommandHandler<InactiveCourseCommand>
{
    private readonly ILogger<InactiveCourseCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public InactiveCourseCommandHandler(ILogger<InactiveCourseCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(InactiveCourseCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start InactiveCourseCommandHandler {@Command}", command);
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

            // Check tutor ownership
            if (course.TutorId != command.TutorId)
            {
                _logger.LogWarning("Unauthorized inactive attempt by user ID: {tutorId} for course ID: {CourseId}", command.TutorId, command.Id);
                return Result.Failure(HttpStatusCode.Unauthorized, new Error("Unauthorized", "Bạn không có quyền thay đổi trạng thái khóa học này"));
            }

            course.Status = nameof(CourseStatus.InActive);
            courseRepo.Update(course);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End InactiveCourseCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in InactiveCourseCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
