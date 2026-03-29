using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseModuleCourseSessions.AddCourseSessionToCourseModule;

public class AddCourseSessionToCourseModuleCommandHandler : ICommandHandler<AddCourseSessionToCourseModuleCommand>
{
    private readonly ILogger<AddCourseSessionToCourseModuleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AddCourseSessionToCourseModuleCommandHandler(ILogger<AddCourseSessionToCourseModuleCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(AddCourseSessionToCourseModuleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start AddCourseSessionToCourseModuleCommandHandler {@Command}", command);
        try
        {
            var courseModuleCourseSessionRepo = _unitOfWork.GetRepository<CourseModuleCourseSession, Guid>();
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();
            var courseSessionRepo = _unitOfWork.GetRepository<CourseSession, Guid>();

            // Check if course module exists
            var courseModuleExists = await courseModuleRepo.AnyAsync(x => x.Id == command.CourseModuleId, cancellationToken);
            if (!courseModuleExists)
            {
                _logger.LogWarning("CourseModule not found with ID: {CourseModuleId}", command.CourseModuleId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseModuleNotFound", "Module khóa học không tồn tại"));
            }

            // Check if course session exists
            var courseSessionExists = await courseSessionRepo.AnyAsync(x => x.Id == command.CourseSessionId, cancellationToken);
            if (!courseSessionExists)
            {
                _logger.LogWarning("CourseSession not found with ID: {CourseSessionId}", command.CourseSessionId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseSessionNotFound", "Buổi học không tồn tại"));
            }

            // Check if relationship already exists
            var relationshipExists = await courseModuleCourseSessionRepo.AnyAsync(
                x => x.CourseModuleId == command.CourseModuleId && x.CourseSessionId == command.CourseSessionId,
                cancellationToken);
            if (relationshipExists)
            {
                _logger.LogWarning("CourseModuleCourseSession already exists for CourseModule: {CourseModuleId} and CourseSession: {CourseSessionId}", 
                    command.CourseModuleId, command.CourseSessionId);
                return Result.Failure(HttpStatusCode.BadRequest, new Error("CourseModuleCourseSessionExists", "Buổi học này đã được thêm vào module"));
            }

            var courseModuleCourseSession = new CourseModuleCourseSession
            {
                CourseModuleId = command.CourseModuleId,
                CourseSessionId = command.CourseSessionId,
                SessionNumber = command.SessionNumber
            };

            courseModuleCourseSessionRepo.Add(courseModuleCourseSession);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End AddCourseSessionToCourseModuleCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in AddCourseSessionToCourseModuleCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
