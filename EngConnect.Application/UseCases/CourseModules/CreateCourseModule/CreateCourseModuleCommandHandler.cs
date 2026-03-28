using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
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

    public async Task<Result> HandleAsync(CreateCourseModuleCommand command,
        CancellationToken cancellationToken = default)
    {
        // Track if we are using a transaction
        Guid? transactionId = null;
        _logger.LogInformation("Start CreateCourseModuleCommandHandler {@Command}", command);
        try
        {
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();
            var courseRepo = _unitOfWork.GetRepository<Course, Guid>();
            var courseCourseModuleRepo = _unitOfWork.GetRepository<CourseCourseModule, Guid>();

            // Check if course exists
            var courseExists = await courseRepo.FindFirstAsync(
                x => x.Id == command.CourseId, 
                tracking: false, 
                cancellationToken: cancellationToken);
            if (ValidationUtil.IsNullOrEmpty(courseExists)) 
            {
                _logger.LogWarning("Course not found with ID: {CourseId}", command.CourseId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseNotFound", "Khóa học không tồn tại"));
            }
            
            // Check status of course
            if (courseExists.Status != nameof(CourseStatus.Published))
            {
                _logger.LogWarning("Course with ID: {CourseId} cannot be updated", command.CourseId);
                return Result.Failure(HttpStatusCode.BadRequest, CourseModuleErrors.CourseModuleIsInUse());
            }
            
            // Check tutor
            var tutorExists = await courseRepo.AnyAsync(
                x => x.Id == command.CourseId && x.TutorId == command.TutorId, cancellationToken);
            
            // Begin transaction
            var transaction = await _unitOfWork.BeginTransactionAsync();
            transactionId = transaction.TransactionId;

            // Create course module 
            if (command.NewCourseModules is { Count: > 0 })
            {
                foreach (var module in command.NewCourseModules)
                {
                    var moduleId = Guid.NewGuid();
                    var courseModule = new CourseModule
                    {
                        Id = moduleId,
                        TutorId = command.TutorId,
                        Title = module.Title,
                        Description = module.Description,
                        Outcomes = module.Outcomes,
                    };

                    courseModuleRepo.Add(courseModule);

                    // Create relationship with course
                    var courseCourseModule = new CourseCourseModule
                    {
                        CourseId = command.CourseId,
                        CourseModuleId = moduleId,
                        ModuleNumber = module.ModuleNumber
                    };
                    courseCourseModuleRepo.Add(courseCourseModule);
                }
            }
            else
            {
                _logger.LogInformation("No new course modules to add for course ID: {CourseId}", command.CourseId);
            }

            // Add course module exist to course
            if (command.CourseModuleIdExists is { Count: > 0 })
            {
                // Check if one or more course module exist
                var existingModuleId = courseCourseModuleRepo.FindAll(x =>
                        x.CourseId == command.CourseId)
                    .Select(x => x.CourseModuleId)
                    .ToList();
                var duplicatedModuleId = command.CourseModuleIdExists
                    .Where(x => existingModuleId.Any(em => em == x.CourseModuleId))
                    .ToList();
                if (duplicatedModuleId.Any())
                {
                    _logger.LogWarning("One or more CourseModules already exist in Course");
                    return Result.Failure(HttpStatusCode.BadRequest, CourseModuleErrors.RelationshipExist());
                }

                foreach (var module in command.CourseModuleIdExists)
                {
                    // Create relationship with course
                    var courseCourseModule = new CourseCourseModule
                    {
                        CourseId = command.CourseId,
                        CourseModuleId = module.CourseModuleId,
                        ModuleNumber = module.ModuleNumber
                    };
                    courseCourseModuleRepo.Add(courseCourseModule);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            // Commit transaction if all operations succeeded
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Committing transaction with {TransactionId}", transactionId);
                await _unitOfWork.CommitTransactionAsync();
            }

            _logger.LogInformation("End CreateCourseModuleCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateCourseModuleCommandHandler: {Message}", ex.Message);
            // Rollback transaction if we started one
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Rolling back transaction with {TransactionId}", transactionId);
                await _unitOfWork.RollbackTransactionAsync();
            }

            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}