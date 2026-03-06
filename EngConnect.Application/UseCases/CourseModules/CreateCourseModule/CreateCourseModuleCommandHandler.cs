using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseModules.CreateCourseModule;

public class CreateCourseModuleCommandHandler : ICommandHandler<CreateCourseModuleCommand>
{
    private readonly ILogger<CreateCourseModuleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseModuleCommandHandler(ILogger<CreateCourseModuleCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateCourseModuleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateCourseModuleCommandHandler {@Command}", command);
        try
        {
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();
            var courseRepo = _unitOfWork.GetRepository<Course, Guid>();

            // Check if course exists
            var courseExists = await courseRepo.AnyAsync(x => x.Id == command.CourseId, cancellationToken);
            if (!courseExists)
            {
                _logger.LogWarning("Course not found with ID: {CourseId}", command.CourseId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseNotFound", "Khóa học không tồn tại"));
            }

            var courseModule = new CourseModule
            {
                CourseId = command.CourseId,
                Title = command.Title,
                Description = command.Description,
                Outcomes = command.Outcomes,
                ModuleNumber = command.ModuleNumber
            };

            courseModuleRepo.Add(courseModule);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End CreateCourseModuleCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateCourseModuleCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
