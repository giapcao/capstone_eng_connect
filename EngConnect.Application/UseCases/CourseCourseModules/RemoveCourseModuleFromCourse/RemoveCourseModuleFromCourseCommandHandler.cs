using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseCourseModules.RemoveCourseModuleFromCourse;

public class RemoveCourseModuleFromCourseCommandHandler : ICommandHandler<RemoveCourseModuleFromCourseCommand>
{
    private readonly ILogger<RemoveCourseModuleFromCourseCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveCourseModuleFromCourseCommandHandler(ILogger<RemoveCourseModuleFromCourseCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(RemoveCourseModuleFromCourseCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start RemoveCourseModuleFromCourseCommandHandler {@Command}", command);
        try
        {
            var courseCourseModuleRepo = _unitOfWork.GetRepository<CourseCourseModule, Guid>();

            var courseCourseModule = await courseCourseModuleRepo.FindSingleAsync(
                x => x.Id == command.Id,
                cancellationToken: cancellationToken);
            if (courseCourseModule == null)
            {
                _logger.LogWarning("CourseCourseModule not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseCourseModuleNotFound", "Liên kết giữa khóa học và module không tồn tại"));
            }

            courseCourseModuleRepo.Delete(courseCourseModule);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End RemoveCourseModuleFromCourseCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in RemoveCourseModuleFromCourseCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
