using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseCourseModules.AddCourseModuleToCourse;

public class AddCourseModuleToCourseCommandHandler : ICommandHandler<AddCourseModuleToCourseCommand>
{
    private readonly ILogger<AddCourseModuleToCourseCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AddCourseModuleToCourseCommandHandler(ILogger<AddCourseModuleToCourseCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(AddCourseModuleToCourseCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start AddCourseModuleToCourseCommandHandler {@Command}", command);
        try
        {
            var courseCourseModuleRepo = _unitOfWork.GetRepository<CourseCourseModule, Guid>();
            var courseRepo = _unitOfWork.GetRepository<Course, Guid>();
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();

            // Check if course exists
            var courseExists = await courseRepo.AnyAsync(x => x.Id == command.CourseId, cancellationToken);
            if (!courseExists)
            {
                _logger.LogWarning("Course not found with ID: {CourseId}", command.CourseId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseNotFound", "Khóa học không tồn tại"));
            }

            // Check if course module exists
            var courseModuleExists = await courseModuleRepo.AnyAsync(x => x.Id == command.CourseModuleId, cancellationToken);
            if (!courseModuleExists)
            {
                _logger.LogWarning("CourseModule not found with ID: {CourseModuleId}", command.CourseModuleId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseModuleNotFound", "Module khóa học không tồn tại"));
            }

            // Check if course-courseModule relationship already exists
            var courseCourseModuleExists = await courseCourseModuleRepo.AnyAsync(
                x => x.CourseId == command.CourseId && x.CourseModuleId == command.CourseModuleId,
                cancellationToken);
            if (courseCourseModuleExists)
            {
                _logger.LogWarning("CourseCourseModule already exists for Course: {CourseId} and CourseModule: {CourseModuleId}", 
                    command.CourseId, command.CourseModuleId);
                return Result.Failure(HttpStatusCode.BadRequest, new Error("CourseCourseModuleExists", "Module này đã được thêm vào khóa học"));
            }

            var courseCourseModule = new CourseCourseModule
            {
                CourseId = command.CourseId,
                CourseModuleId = command.CourseModuleId,
                ModuleNumber = command.ModuleNumber
            };

            courseCourseModuleRepo.Add(courseCourseModule);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End AddCourseModuleToCourseCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in AddCourseModuleToCourseCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
