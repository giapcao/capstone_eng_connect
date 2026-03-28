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

namespace EngConnect.Application.UseCases.CourseSessions.CreateCourseSession;

public class CreateCourseSessionCommandHandler : ICommandHandler<CreateCourseSessionCommand>
{
    private readonly ILogger<CreateCourseSessionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseSessionCommandHandler(ILogger<CreateCourseSessionCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateCourseSessionCommand command,
        CancellationToken cancellationToken = default)
    {
        //Track if we are using a transaction
        Guid? transactionId = null;
        _logger.LogInformation("Start CreateCourseSessionCommandHandler {@Command}", command);
        try
        {
            var courseSessionRepo = _unitOfWork.GetRepository<CourseSession, Guid>();
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();
            var courseModuleCourseSessionRepo = _unitOfWork.GetRepository<CourseModuleCourseSession, Guid>();

            // Check if course module exists
            var courseModule =
                await courseModuleRepo.FindFirstAsync(x => 
                    x.Id == command.CourseModuleId, 
                    tracking: false, cancellationToken);
            if (ValidationUtil.IsNullOrEmpty(courseModule))
            {
                _logger.LogWarning("Course module not found with ID: {CourseModuleId}", command.CourseModuleId);
                return Result.Failure(HttpStatusCode.BadRequest,
                    new Error("CourseModuleNotFound", "Module không tồn tại"));
            }
            
            // Check status of course
            var listCourse = await _unitOfWork.GetRepository<CourseCourseModule, Guid>()
                .FindAll(x => x.CourseModuleId == command.CourseModuleId, cancellationToken:cancellationToken)
                .Include(x => x.Course)
                .ToListAsync(cancellationToken);

            if (listCourse.Any(x => x.Course.Status != nameof(CourseStatus.InActive)))
            {
                _logger.LogWarning("Course with ID: {CourseId} cannot be updated", listCourse.Select(x => x.CourseId));
                return Result.Failure(HttpStatusCode.BadRequest, CourseSessionErrors.CourseSessionIsInUse());
            }
            

            // Begin transaction
            var transaction = await _unitOfWork.BeginTransactionAsync();
            transactionId = transaction.TransactionId;

            // Create course session 
            if (command.NewCourseSessions is { Count: > 0 })
            {
                foreach (var session in command.NewCourseSessions)
                {
                    var sessionId = Guid.NewGuid();
                    var courseSession = new CourseSession
                    {
                        Id = sessionId,
                        TutorId = command.TutorId,
                        Title = session.Title,
                        Description = session.Description,
                        Outcomes = session.Outcomes,
                    };

                    courseSessionRepo.Add(courseSession);

                    // Create relationship with course module
                    var courseModuleCourseSession = new CourseModuleCourseSession
                    {
                        CourseModuleId = command.CourseModuleId,
                        CourseSessionId = sessionId,
                        SessionNumber = session.SessionNumber
                    };
                    courseModuleCourseSessionRepo.Add(courseModuleCourseSession);
                }
            }
            else
            {
                _logger.LogInformation("No new course sessions to add for course module ID: {CourseModuleId}",
                    command.CourseModuleId);
            }

            // Add course session exist to course module
            if (command.CourseSessionIdExists is { Count: > 0 })
            {
                // Check if any course session exist to add to course module
                var existingSessionIds = await courseModuleCourseSessionRepo.FindAll(x =>
                        x.CourseModuleId == command.CourseModuleId)
                    .Select(x => x.CourseSessionId)
                    .ToListAsync(cancellationToken: cancellationToken);

                var duplicatedSessionIds = command.CourseSessionIdExists
                    .Where(s => existingSessionIds.Any(id => id == s.CourseSessionId))
                    .ToList();

                if (duplicatedSessionIds.Any())
                {
                    _logger.LogWarning("One or more CourseSession already exist in course module");
                    return Result.Failure(HttpStatusCode.BadRequest, CourseSessionErrors.RelationshipExist());
                }

                foreach (var session in command.CourseSessionIdExists)
                {
                    // Create relationship with course module
                    var courseModuleCourseSession = new CourseModuleCourseSession
                    {
                        CourseModuleId = command.CourseModuleId,
                        CourseSessionId = session.CourseSessionId,
                        SessionNumber = session.SessionNumber
                    };
                    courseModuleCourseSessionRepo.Add(courseModuleCourseSession);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            //Commit transaction
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Committing transaction with {TransactionId}", transactionId);
                await _unitOfWork.CommitTransactionAsync();
            }

            _logger.LogInformation("End CreateCourseSessionCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateCourseSessionCommandHandler: {Message}", ex.Message);
            //Rollback transaction if we started one
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Rolling back transaction with {TransactionId}", transactionId);
                await _unitOfWork.RollbackTransactionAsync();
            }

            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}