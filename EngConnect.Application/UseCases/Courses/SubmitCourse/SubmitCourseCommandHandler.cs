using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Courses.SubmitCourse;

public class SubmitCourseCommandHandler: ICommandHandler<SubmitCourseCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SubmitCourseCommandHandler> _logger;

    public SubmitCourseCommandHandler(IUnitOfWork unitOfWork, ILogger<SubmitCourseCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(SubmitCourseCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start SubmitCourseCommandHandler {@Command}", command);
        try
        { 
            //Check course exist
            var courseRepo = _unitOfWork.GetRepository<Course, Guid>();
            var course = await courseRepo.FindFirstAsync(x => x.Id == command.CourseId, cancellationToken: cancellationToken);
            if (ValidationUtil.IsNullOrEmpty(course))            {
                _logger.LogWarning("Course not found with id: {courseId}", command.CourseId);
                return Result.Failure(HttpStatusCode.NotFound, CourseErrors.CourseNotFound());
            }
            
            //Check course module exist
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();
            var courseModules = courseModuleRepo.FindAll(
                x => x.CourseId == command.CourseId,
                tracking: false,
                cancellationToken: cancellationToken);
            if (ValidationUtil.IsNullOrEmpty(courseModules))
            {
                _logger.LogWarning("Not found course module in course {courseId}", command.CourseId);
                return Result.Failure(HttpStatusCode.NotFound, CourseErrors.CourseModuleNotFound());
            }

            foreach (var module in courseModules)
            {
                //Check course session exist
                var courseSessionRepo = _unitOfWork.GetRepository<CourseSession, Guid>();
                var courseSessions = courseSessionRepo.FindAll(
                    x => x.ModuleId == module.Id,
                    tracking: false,
                    cancellationToken: cancellationToken);
                if (ValidationUtil.IsNullOrEmpty(courseSessions))                {
                    _logger.LogWarning("Not found course session in course module {courseModuleId}", module.Id);
                    return Result.Failure(HttpStatusCode.NotFound, CourseModuleErrors.CourseSessionNotFound());
                }
                foreach (var session in courseSessions)
                {
                    //Check course session resource exist
                    var courseResourcetRepo = _unitOfWork.GetRepository<CourseResource, Guid>();
                    var courseSessionContents = courseResourcetRepo.FindAll(
                        x => x.SessionId == session.Id,
                        tracking: false,
                        cancellationToken: cancellationToken);
                    if (ValidationUtil.IsNullOrEmpty(courseSessionContents))                    {
                        _logger.LogWarning("Not found course session content in course session {courseSessionId}", session.Id);
                        return Result.Failure(HttpStatusCode.NotFound, CourseSessionErrors.CourseResourceNotFound());
                    }
                }
            }
            //Update course status
            course.Status = nameof(CourseStatus.Pending);
            courseRepo.Update(course);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("End SubmitCourseCommandHandler");
            return Result.Success();
        }catch (Exception ex)
        {
            _logger.LogError(ex, "Error in SubmitCourseCommandHandler {@Command}", command);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}