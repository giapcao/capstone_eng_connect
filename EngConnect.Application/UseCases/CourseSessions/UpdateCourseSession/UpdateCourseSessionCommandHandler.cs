using System.Net;
using EngConnect.Application.UseCases.CourseSessions.Common;
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

namespace EngConnect.Application.UseCases.CourseSessions.UpdateCourseSession;

public class UpdateCourseSessionCommandHandler : ICommandHandler<UpdateCourseSessionCommand, GetCourseSessionResponse>
{
    private readonly ILogger<UpdateCourseSessionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseSessionCommandHandler(ILogger<UpdateCourseSessionCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetCourseSessionResponse>> HandleAsync(UpdateCourseSessionCommand command,
        CancellationToken cancellationToken = default)
    {
        Guid? transactionId = null;
        _logger.LogInformation("Start UpdateCourseSessionCommandHandler {@Command}", command);
        try
        {
            var courseSessionRepo = _unitOfWork.GetRepository<CourseSession, Guid>();
            var courseModuleCourseSessionRepo = _unitOfWork.GetRepository<CourseModuleCourseSession, Guid>();

            var courseSession = await courseSessionRepo.FindSingleAsync(
                x => x.Id == command.Id,
                cancellationToken: cancellationToken);

            if (courseSession == null)
            {
                _logger.LogWarning("CourseSession not found with ID: {Id}", command.Id);
                return Result.Failure<GetCourseSessionResponse>(HttpStatusCode.NotFound,
                    new Error("CourseSessionNotFound", "Session khong ton tai"));
            }

            var relations = await courseModuleCourseSessionRepo.FindAll(x => x.CourseSessionId == command.Id)
                .Include(x => x.CourseModule)
                    .ThenInclude(x => x.CourseCourseModules)
                        .ThenInclude(x => x.Course)
                .ToListAsync(cancellationToken);

            var hasPublishedCourse = relations
                .SelectMany(x => x.CourseModule.CourseCourseModules)
                .Any(x => x.Course.Status == nameof(CourseStatus.Published));

            if (hasPublishedCourse)
            {
                _logger.LogWarning("CourseSession with ID: {Id} cannot be updated because it's in use by a Published course", command.Id);
                return Result.Failure<GetCourseSessionResponse>(HttpStatusCode.BadRequest,
                    CourseSessionErrors.CourseSessionIsInUse());
            }

            if (command.CourseModuleId.HasValue && relations.All(x => x.CourseModuleId != command.CourseModuleId.Value))
            {
                return Result.Failure<GetCourseSessionResponse>(HttpStatusCode.BadRequest,
                    CommonErrors.ValidationFailed("CourseModuleId does not reference this session"));
            }

            var transaction = await _unitOfWork.BeginTransactionAsync();
            transactionId = transaction.TransactionId;

            var newSession = new CourseSession
            {
                Id = Guid.NewGuid(),
                TutorId = courseSession.TutorId,
                ParentSessionId = courseSession.Id,
                Title = command.Title,
                Description = command.Description,
                Outcomes = command.Outcomes
            };

            courseSessionRepo.Add(newSession);

            var targetRelations = command.CourseModuleId.HasValue
                ? relations.Where(x => x.CourseModuleId == command.CourseModuleId.Value).ToList()
                : relations;

            foreach (var relation in targetRelations)
            {
                relation.CourseSessionId = newSession.Id;
                courseModuleCourseSessionRepo.Update(relation);
            }

            await _unitOfWork.SaveChangesAsync();

            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                await _unitOfWork.CommitTransactionAsync();
            }

            var targetModuleId = targetRelations.Select(x => x.CourseModuleId).FirstOrDefault();
            return Result.Success(new GetCourseSessionResponse
            {
                Id = newSession.Id,
                ModuleId = targetModuleId,
                ParentSessionId = newSession.ParentSessionId,
                Title = newSession.Title,
                Description = newSession.Description,
                Outcomes = newSession.Outcomes,
                CreatedAt = newSession.CreatedAt,
                UpdatedAt = newSession.UpdatedAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateCourseSessionCommandHandler: {Message}", ex.Message);
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                await _unitOfWork.RollbackTransactionAsync();
            }

            return Result.Failure<GetCourseSessionResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
