using System.Net;
using EngConnect.Application.UseCases.CourseModules.Common;
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

public class UpdateCourseModuleCommandHandler : ICommandHandler<UpdateCourseModuleCommand, GetCourseModuleResponse>
{
    private readonly ILogger<UpdateCourseModuleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseModuleCommandHandler(ILogger<UpdateCourseModuleCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetCourseModuleResponse>> HandleAsync(UpdateCourseModuleCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateCourseModuleCommandHandler {@Command}", command);
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
                return Result.Failure<GetCourseModuleResponse>(HttpStatusCode.NotFound,
                    new Error("CourseModuleNotFound", "Course module not found"));
            }

            var listCourse = await courseCourseModuleRepo.FindAll(x => x.CourseModuleId == command.Id)
                .Include(x => x.Course)
                .ToListAsync(cancellationToken);

            if (listCourse.Any(x => x.Course.Status == nameof(CourseStatus.Published)))
            {
                _logger.LogWarning("CourseModule with ID: {Id} cannot be updated because it's in use by a Published course", command.Id);
                return Result.Failure<GetCourseModuleResponse>(HttpStatusCode.BadRequest,
                    CourseModuleErrors.CourseModuleIsInUse());
            }

            courseModule.Title = command.Title;
            courseModule.Description = command.Description;
            courseModule.Outcomes = command.Outcomes;

            courseModuleRepo.Update(courseModule);
            await _unitOfWork.SaveChangesAsync();

            var targetCourseId = listCourse.Select(x => x.CourseId).FirstOrDefault();
            if (targetCourseId == Guid.Empty)
            {
                return Result.Success(new GetCourseModuleResponse
                {
                    Id = courseModule.Id,
                    CourseId = Guid.Empty,
                    Title = courseModule.Title,
                    Description = courseModule.Description,
                    Outcomes = courseModule.Outcomes,
                    CreatedAt = courseModule.CreatedAt,
                    UpdatedAt = courseModule.UpdatedAt
                });
            }

            await courseCourseModuleRepo.FindAll(x =>
                    x.CourseId == targetCourseId && x.CourseModuleId == courseModule.Id)
                .FirstOrDefaultAsync(cancellationToken);

            _logger.LogInformation("End UpdateCourseModuleCommandHandler");
            return Result.Success(new GetCourseModuleResponse
            {
                Id = courseModule.Id,
                CourseId = targetCourseId,
                Title = courseModule.Title,
                Description = courseModule.Description,
                Outcomes = courseModule.Outcomes,
                CreatedAt = courseModule.CreatedAt,
                UpdatedAt = courseModule.UpdatedAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateCourseModuleCommandHandler: {Message}", ex.Message);
            return Result.Failure<GetCourseModuleResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
