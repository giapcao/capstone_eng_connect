using System.Net;
using EngConnect.Application.UseCases.CourseSessions.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseSessions.CreateCourseSession;

public class CreateCourseSessionCommandHandler : ICommandHandler<CreateCourseSessionCommand, GetCourseSessionListResponse>
{
    private readonly ILogger<CreateCourseSessionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseSessionCommandHandler(ILogger<CreateCourseSessionCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetCourseSessionListResponse>> HandleAsync(CreateCourseSessionCommand command,
        CancellationToken cancellationToken = default)
    {
        Guid? transactionId = null;
        _logger.LogInformation("Start CreateCourseSessionCommandHandler {@Command}", command);
        try
        {
            var courseSessionRepo = _unitOfWork.GetRepository<CourseSession, Guid>();
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();
            var courseModuleCourseSessionRepo = _unitOfWork.GetRepository<CourseModuleCourseSession, Guid>();

            var courseModuleExists = await courseModuleRepo.AnyAsync(x => x.Id == command.CourseModuleId, cancellationToken);
            if (!courseModuleExists)
            {
                _logger.LogWarning("Course module not found with ID: {CourseModuleId}", command.CourseModuleId);
                return Result.Failure<GetCourseSessionListResponse>(HttpStatusCode.NotFound,
                    new Error("CourseModuleNotFound", "Module khong ton tai"));
            }

            var transaction = await _unitOfWork.BeginTransactionAsync();
            transactionId = transaction.TransactionId;

            if (command.NewCourseSessions is { Count: > 0 })
            {
                foreach (var session in command.NewCourseSessions)
                {
                    var sessionId = Guid.NewGuid();
                    courseSessionRepo.Add(new CourseSession
                    {
                        Id = sessionId,
                        TutorId = command.TutorId,
                        Title = session.Title,
                        Description = session.Description,
                        Outcomes = session.Outcomes
                    });

                    courseModuleCourseSessionRepo.Add(new CourseModuleCourseSession
                    {
                        Id = Guid.NewGuid(),
                        CourseModuleId = command.CourseModuleId,
                        CourseSessionId = sessionId,
                        SessionNumber = session.SessionNumber
                    });
                }
            }

            if (command.CourseSessionIdExists is { Count: > 0 })
            {
                var existingSessionIds = await courseModuleCourseSessionRepo.FindAll(x => x.CourseModuleId == command.CourseModuleId)
                    .Select(x => x.CourseSessionId)
                    .ToListAsync(cancellationToken);

                var duplicatedSessionIds = command.CourseSessionIdExists
                    .Where(x => existingSessionIds.Contains(x.CourseSessionId))
                    .ToList();

                if (duplicatedSessionIds.Count > 0)
                {
                    if (ValidationUtil.IsNotNullOrEmpty(transactionId))
                    {
                        await _unitOfWork.RollbackTransactionAsync();
                    }

                    return Result.Failure<GetCourseSessionListResponse>(HttpStatusCode.BadRequest,
                        CourseSessionErrors.RelationshipExist());
                }

                foreach (var session in command.CourseSessionIdExists)
                {
                    courseModuleCourseSessionRepo.Add(new CourseModuleCourseSession
                    {
                        Id = Guid.NewGuid(),
                        CourseModuleId = command.CourseModuleId,
                        CourseSessionId = session.CourseSessionId,
                        SessionNumber = session.SessionNumber
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync();

            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Committing transaction with {TransactionId}", transactionId);
                await _unitOfWork.CommitTransactionAsync();
            }

            var courseSessions = await courseModuleCourseSessionRepo.FindAll(x => x.CourseModuleId == command.CourseModuleId)
                .Include(x => x.CourseSession)
                .OrderBy(x => x.SessionNumber)
                .ThenBy(x => x.CreatedAt)
                .Select(x => new GetCourseSessionResponse
                {
                    Id = x.CourseSessionId,
                    ModuleId = x.CourseModuleId,
                    Title = x.CourseSession.Title,
                    Description = x.CourseSession.Description,
                    Outcomes = x.CourseSession.Outcomes,
                    SessionNumber = x.SessionNumber,
                    CreatedAt = x.CourseSession.CreatedAt,
                    UpdatedAt = x.CourseSession.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            _logger.LogInformation("End CreateCourseSessionCommandHandler");
            return Result.Success(new GetCourseSessionListResponse
            {
                ModuleId = command.CourseModuleId,
                CourseSessions = courseSessions
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateCourseSessionCommandHandler: {Message}", ex.Message);
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Rolling back transaction with {TransactionId}", transactionId);
                await _unitOfWork.RollbackTransactionAsync();
            }

            return Result.Failure<GetCourseSessionListResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
