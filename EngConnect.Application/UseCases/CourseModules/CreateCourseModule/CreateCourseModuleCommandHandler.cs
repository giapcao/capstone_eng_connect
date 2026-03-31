using System.Net;
using EngConnect.Application.UseCases.CourseModules.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseModules.CreateCourseModule;

public class CreateCourseModuleCommandHandler : ICommandHandler<CreateCourseModuleCommand, GetCourseModuleListResponse>
{
    private readonly ILogger<CreateCourseModuleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseModuleCommandHandler(ILogger<CreateCourseModuleCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetCourseModuleListResponse>> HandleAsync(CreateCourseModuleCommand command,
        CancellationToken cancellationToken = default)
    {
        Guid? transactionId = null;
        _logger.LogInformation("Start CreateCourseModuleCommandHandler {@Command}", command);
        try
        {
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();
            var courseRepo = _unitOfWork.GetRepository<Course, Guid>();
            var courseCourseModuleRepo = _unitOfWork.GetRepository<CourseCourseModule, Guid>();

            var course = await courseRepo.FindSingleAsync(x => x.Id == command.CourseId, cancellationToken: cancellationToken);
            if (course == null)
            {
                _logger.LogWarning("Course not found with ID: {CourseId}", command.CourseId);
                return Result.Failure<GetCourseModuleListResponse>(HttpStatusCode.NotFound,
                    new Error("CourseNotFound", "Course not found"));
            }

            if (course.Status == nameof(CourseStatus.Published))
            {
                _logger.LogWarning("Course modules cannot be changed because course {CourseId} is published", command.CourseId);
                return Result.Failure<GetCourseModuleListResponse>(HttpStatusCode.BadRequest,
                    CourseErrors.PublishedCourseCannotBeUpdated());
            }

            var transaction = await _unitOfWork.BeginTransactionAsync();
            transactionId = transaction.TransactionId;

            if (command.NewCourseModules is { Count: > 0 })
            {
                foreach (var module in command.NewCourseModules)
                {
                    var moduleId = Guid.NewGuid();
                    courseModuleRepo.Add(new CourseModule
                    {
                        Id = moduleId,
                        TutorId = command.TutorId,
                        Title = module.Title,
                        Description = module.Description,
                        Outcomes = module.Outcomes
                    });

                    courseCourseModuleRepo.Add(new CourseCourseModule
                    {
                        Id = Guid.NewGuid(),
                        CourseId = command.CourseId,
                        CourseModuleId = moduleId,
                        ModuleNumber = module.ModuleNumber
                    });
                }
            }

            if (command.CourseModuleIdExists is { Count: > 0 })
            {
                var existingModuleIds = await courseCourseModuleRepo.FindAll(x => x.CourseId == command.CourseId)
                    .Select(x => x.CourseModuleId)
                    .ToListAsync(cancellationToken);

                var duplicatedModuleIds = command.CourseModuleIdExists
                    .Where(x => existingModuleIds.Contains(x.CourseModuleId))
                    .ToList();

                if (duplicatedModuleIds.Count > 0)
                {
                    if (ValidationUtil.IsNotNullOrEmpty(transactionId))
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                    }

                    return Result.Failure<GetCourseModuleListResponse>(HttpStatusCode.BadRequest,
                        CourseModuleErrors.RelationshipExist());
                }

                foreach (var module in command.CourseModuleIdExists)
                {
                    courseCourseModuleRepo.Add(new CourseCourseModule
                    {
                        Id = Guid.NewGuid(),
                        CourseId = command.CourseId,
                        CourseModuleId = module.CourseModuleId,
                        ModuleNumber = module.ModuleNumber
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync();

            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Committing transaction with {TransactionId}", transactionId);
                await _unitOfWork.CommitTransactionAsync();
            }

            var courseModules = await courseCourseModuleRepo.FindAll(x => x.CourseId == command.CourseId)
                .Include(x => x.CourseModule)
                .OrderBy(x => x.ModuleNumber)
                .ThenBy(x => x.CreatedAt)
                .Select(x => new GetCourseModuleResponse
                {
                    Id = x.CourseModuleId,
                    CourseId = x.CourseId,
                    Title = x.CourseModule.Title,
                    Description = x.CourseModule.Description,
                    Outcomes = x.CourseModule.Outcomes,
                    ModuleNumber = x.ModuleNumber,
                    CreatedAt = x.CourseModule.CreatedAt,
                    UpdatedAt = x.CourseModule.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation("End CreateCourseModuleCommandHandler");
            return Result.Success(new GetCourseModuleListResponse
            {
                CourseId = command.CourseId,
                CourseModules = courseModules
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateCourseModuleCommandHandler: {Message}", ex.Message);
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Rolling back transaction with {TransactionId}", transactionId);
                await _unitOfWork.RollbackTransactionAsync();
            }

            return Result.Failure<GetCourseModuleListResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
