using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseSessionCourseResources.RemoveCourseResourceFromCourseSession;

public class RemoveCourseResourceFromCourseSessionCommandHandler : ICommandHandler<RemoveCourseResourceFromCourseSessionCommand>
{
    private readonly ILogger<RemoveCourseResourceFromCourseSessionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveCourseResourceFromCourseSessionCommandHandler(ILogger<RemoveCourseResourceFromCourseSessionCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(RemoveCourseResourceFromCourseSessionCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start RemoveCourseResourceFromCourseSessionCommandHandler {@Command}", command);
        try
        {
            var courseSessionCourseResourceRepo = _unitOfWork.GetRepository<CourseSessionCourseResource, Guid>();

            var courseSessionCourseResource = await courseSessionCourseResourceRepo.FindSingleAsync(
                x => x.Id == command.Id,
                cancellationToken: cancellationToken);
            if (courseSessionCourseResource == null)
            {
                _logger.LogWarning("CourseSessionCourseResource not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseSessionCourseResourceNotFound", "Liên kết giữa buổi học và tài nguyên không tồn tại"));
            }

            courseSessionCourseResourceRepo.Delete(courseSessionCourseResource);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End RemoveCourseResourceFromCourseSessionCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in RemoveCourseResourceFromCourseSessionCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
