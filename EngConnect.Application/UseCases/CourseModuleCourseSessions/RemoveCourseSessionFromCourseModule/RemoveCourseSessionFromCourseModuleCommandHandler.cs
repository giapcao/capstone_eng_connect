using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseModuleCourseSessions.RemoveCourseSessionFromCourseModule;

public class RemoveCourseSessionFromCourseModuleCommandHandler : ICommandHandler<RemoveCourseSessionFromCourseModuleCommand>
{
    private readonly ILogger<RemoveCourseSessionFromCourseModuleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveCourseSessionFromCourseModuleCommandHandler(ILogger<RemoveCourseSessionFromCourseModuleCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(RemoveCourseSessionFromCourseModuleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start RemoveCourseSessionFromCourseModuleCommandHandler {@Command}", command);
        try
        {
            var courseModuleCourseSessionRepo = _unitOfWork.GetRepository<CourseModuleCourseSession, Guid>();

            var courseModuleCourseSession = await courseModuleCourseSessionRepo.FindSingleAsync(
                x => x.Id == command.Id,
                cancellationToken: cancellationToken);
            if (courseModuleCourseSession == null)
            {
                _logger.LogWarning("CourseModuleCourseSession not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseModuleCourseSessionNotFound", "Liên kết giữa module và buổi học không tồn tại"));
            }

            courseModuleCourseSessionRepo.Delete(courseModuleCourseSession);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End RemoveCourseSessionFromCourseModuleCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in RemoveCourseSessionFromCourseModuleCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
