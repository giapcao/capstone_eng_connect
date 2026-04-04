using System.Net;
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
        Guid? transactionId = null;
        _logger.LogInformation("Start AddCourseModuleToCourseCommandHandler {@Command}", command);
        try
        {
            var courseCourseModuleRepo = _unitOfWork.GetRepository<CourseCourseModule, Guid>();
            var courseRepo = _unitOfWork.GetRepository<Course, Guid>();
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();
            var courseModuleCourseSessionRepo = _unitOfWork.GetRepository<CourseModuleCourseSession, Guid>();

            var course = await courseRepo.FindSingleAsync(x => x.Id == command.CourseId, cancellationToken: cancellationToken);
            if (course == null)
            {
                _logger.LogWarning("Course not found with ID: {CourseId}", command.CourseId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseNotFound", "Khóa học không tồn tại"));
            }

            if (course.Status == nameof(CourseStatus.Published))
            {
                return Result.Failure(HttpStatusCode.BadRequest, CourseErrors.PublishedCourseCannotBeUpdated());
            }

            var sourceModule = await courseModuleRepo.FindAll(x => x.Id == command.CourseModuleId)
                .Include(x => x.CourseModuleCourseSessions)
                .FirstOrDefaultAsync(cancellationToken);
            if (sourceModule == null)
            {
                _logger.LogWarning("CourseModule not found with ID: {CourseModuleId}", command.CourseModuleId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseModuleNotFound", "Module khóa học không tồn tại"));
            }

            var relationshipExists = await courseCourseModuleRepo.AnyAsync(
                x => x.CourseId == command.CourseId && x.CourseModuleId == command.CourseModuleId,
                cancellationToken);
            if (relationshipExists)
            {
                _logger.LogWarning("CourseCourseModule already exists for Course: {CourseId} and CourseModule: {CourseModuleId}",
                    command.CourseId, command.CourseModuleId);
                return Result.Failure(HttpStatusCode.BadRequest, new Error("CourseCourseModuleExists", "Module này đã được thêm vào khóa học"));
            }

            var transaction = await _unitOfWork.BeginTransactionAsync();
            transactionId = transaction.TransactionId;

            var clonedModuleId = Guid.NewGuid();
            courseModuleRepo.Add(new CourseModule
            {
                Id = clonedModuleId,
                TutorId = sourceModule.TutorId,
                ParentModuleId = sourceModule.Id,
                Title = sourceModule.Title,
                Description = sourceModule.Description,
                Outcomes = sourceModule.Outcomes
            });

            foreach (var sourceModuleSession in sourceModule.CourseModuleCourseSessions.OrderBy(x => x.SessionNumber).ThenBy(x => x.CreatedAt))
            {
                courseModuleCourseSessionRepo.Add(new CourseModuleCourseSession
                {
                    Id = Guid.NewGuid(),
                    CourseModuleId = clonedModuleId,
                    CourseSessionId = sourceModuleSession.CourseSessionId,
                    SessionNumber = sourceModuleSession.SessionNumber
                });
            }

            courseCourseModuleRepo.Add(new CourseCourseModule
            {
                Id = Guid.NewGuid(),
                CourseId = command.CourseId,
                CourseModuleId = clonedModuleId,
                ModuleNumber = command.ModuleNumber
            });

            await _unitOfWork.SaveChangesAsync();

            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                await _unitOfWork.CommitTransactionAsync();
            }

            _logger.LogInformation("End AddCourseModuleToCourseCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in AddCourseModuleToCourseCommandHandler: {Message}", ex.Message);
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                await _unitOfWork.RollbackTransactionAsync();
            }

            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
