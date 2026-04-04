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

namespace EngConnect.Application.UseCases.CourseModuleCourseSessions.AddCourseSessionToCourseModule;

public class AddCourseSessionToCourseModuleCommandHandler : ICommandHandler<AddCourseSessionToCourseModuleCommand>
{
    private readonly ILogger<AddCourseSessionToCourseModuleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AddCourseSessionToCourseModuleCommandHandler(ILogger<AddCourseSessionToCourseModuleCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(AddCourseSessionToCourseModuleCommand command, CancellationToken cancellationToken = default)
    {
        Guid? transactionId = null;
        _logger.LogInformation("Start AddCourseSessionToCourseModuleCommandHandler {@Command}", command);
        try
        {
            var courseModuleCourseSessionRepo = _unitOfWork.GetRepository<CourseModuleCourseSession, Guid>();
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();
            var courseSessionRepo = _unitOfWork.GetRepository<CourseSession, Guid>();
            var courseSessionCourseResourceRepo = _unitOfWork.GetRepository<CourseSessionCourseResource, Guid>();

            var courseModule = await courseModuleRepo.FindAll(x => x.Id == command.CourseModuleId)
                .Include(x => x.CourseCourseModules)
                    .ThenInclude(x => x.Course)
                .FirstOrDefaultAsync(cancellationToken);
            if (courseModule == null)
            {
                _logger.LogWarning("CourseModule not found with ID: {CourseModuleId}", command.CourseModuleId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseModuleNotFound", "Module khóa học không tồn tại"));
            }

            if (courseModule.CourseCourseModules.Any(x => x.Course.Status == nameof(CourseStatus.Published)))
            {
                return Result.Failure(HttpStatusCode.BadRequest, CourseErrors.PublishedCourseCannotBeUpdated());
            }

            var sourceSession = await courseSessionRepo.FindAll(x => x.Id == command.CourseSessionId)
                .Include(x => x.CourseSessionCourseResources)
                    .ThenInclude(x => x.CourseResource)
                .FirstOrDefaultAsync(cancellationToken);
            if (sourceSession == null)
            {
                _logger.LogWarning("CourseSession not found with ID: {CourseSessionId}", command.CourseSessionId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseSessionNotFound", "Buổi học không tồn tại"));
            }

            var relationshipExists = await courseModuleCourseSessionRepo.AnyAsync(
                x => x.CourseModuleId == command.CourseModuleId && x.CourseSessionId == command.CourseSessionId,
                cancellationToken);
            if (relationshipExists)
            {
                _logger.LogWarning("CourseModuleCourseSession already exists for CourseModule: {CourseModuleId} and CourseSession: {CourseSessionId}",
                    command.CourseModuleId, command.CourseSessionId);
                return Result.Failure(HttpStatusCode.BadRequest, new Error("CourseModuleCourseSessionExists", "Buổi học này đã được thêm vào module"));
            }

            var transaction = await _unitOfWork.BeginTransactionAsync();
            transactionId = transaction.TransactionId;

            var clonedSessionId = Guid.NewGuid();
            courseSessionRepo.Add(new CourseSession
            {
                Id = clonedSessionId,
                TutorId = sourceSession.TutorId,
                ParentSessionId = sourceSession.Id,
                Title = sourceSession.Title,
                Description = sourceSession.Description,
                Outcomes = sourceSession.Outcomes
            });

            courseModuleCourseSessionRepo.Add(new CourseModuleCourseSession
            {
                Id = Guid.NewGuid(),
                CourseModuleId = command.CourseModuleId,
                CourseSessionId = clonedSessionId,
                SessionNumber = command.SessionNumber
            });

            foreach (var sourceSessionResource in sourceSession.CourseSessionCourseResources.OrderBy(x => x.CreatedAt))
            {
                courseSessionCourseResourceRepo.Add(new CourseSessionCourseResource
                {
                    Id = Guid.NewGuid(),
                    CourseSessionId = clonedSessionId,
                    CourseResourceId = sourceSessionResource.CourseResourceId
                });
            }

            await _unitOfWork.SaveChangesAsync();

            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                await _unitOfWork.CommitTransactionAsync();
            }

            _logger.LogInformation("End AddCourseSessionToCourseModuleCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in AddCourseSessionToCourseModuleCommandHandler: {Message}", ex.Message);
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                await _unitOfWork.RollbackTransactionAsync();
            }

            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
