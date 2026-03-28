using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
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

            // Check status of courses that use this module
            var listCourse = await _unitOfWork.GetRepository<CourseCourseModule, Guid>()
                .FindAll(x => x.CourseModuleId == command.Id)
                .Include(x => x.Course)
                .ToListAsync(cancellationToken);

            if (listCourse.Any(x => x.Course.Status == nameof(CourseStatus.Published)))
            {
                _logger.LogWarning("CourseModule with ID: {Id} cannot be updated because it's in use by a Published course", command.Id);
                return Result.Failure(HttpStatusCode.BadRequest, CourseModuleErrors.CourseModuleIsInUse());
            }

            courseModule.Title = command.Title;
            courseModule.Description = command.Description;
            courseModule.Outcomes = command.Outcomes;

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
