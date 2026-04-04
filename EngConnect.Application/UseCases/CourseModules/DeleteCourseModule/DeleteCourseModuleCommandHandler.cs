using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseModules.DeleteCourseModule;

public class DeleteCourseModuleCommandHandler : ICommandHandler<DeleteCourseModuleCommand>
{
    private readonly ILogger<DeleteCourseModuleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourseModuleCommandHandler(ILogger<DeleteCourseModuleCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeleteCourseModuleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteCourseModuleCommandHandler {@Command}", command);
        try
        {
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();
            var courseCourseModuleRepo = _unitOfWork.GetRepository<CourseCourseModule, Guid>();

            var courseModule = await courseModuleRepo.FindSingleAsync(
                x => x.Id == command.Id,
                cancellationToken: cancellationToken);
            if (courseModule == null)
            {
                _logger.LogWarning("CourseModule not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseModuleNotFound", "Module không tồn tại"));
            }

            var relations = await courseCourseModuleRepo.FindAll(x => x.CourseModuleId == command.Id)
                .ToListAsync(cancellationToken);

            foreach (var relation in relations)
            {
                courseCourseModuleRepo.Delete(relation);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End DeleteCourseModuleCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteCourseModuleCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
