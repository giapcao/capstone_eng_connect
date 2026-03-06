using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseModules.UpdateCourseModule;

public class UpdateCourseModuleCommandHandler : ICommandHandler<UpdateCourseModuleCommand>
{
    private readonly ILogger<UpdateCourseModuleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseModuleCommandHandler(ILogger<UpdateCourseModuleCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateCourseModuleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateCourseModuleCommandHandler {@Command}", command);
        try
        {
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();

            var courseModule = await courseModuleRepo.FindSingleAsync(
                x => x.Id == command.Id,
                cancellationToken: cancellationToken);
            if (courseModule == null)
            {
                _logger.LogWarning("CourseModule not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseModuleNotFound", "Module không tồn tại"));
            }

            courseModule.Title = command.Title;
            courseModule.Description = command.Description;
            courseModule.Outcomes = command.Outcomes;
            courseModule.ModuleNumber = command.ModuleNumber;

            courseModuleRepo.Update(courseModule);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdateCourseModuleCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateCourseModuleCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
