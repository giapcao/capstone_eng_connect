using System.Net;
using EngConnect.Application.UseCases.CourseSessions.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
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

            courseSession.Title = command.Title;
            courseSession.Description = command.Description;
            courseSession.Outcomes = command.Outcomes;

            courseSessionRepo.Update(courseSession);
            await _unitOfWork.SaveChangesAsync();

            var targetModuleId = relations.Select(x => x.CourseModuleId).FirstOrDefault();
            if (targetModuleId == Guid.Empty)
            {
                return Result.Success(new GetCourseSessionResponse
                {
                    Id = courseSession.Id,
                    ModuleId = Guid.Empty,
                    Title = courseSession.Title,
                    Description = courseSession.Description,
                    Outcomes = courseSession.Outcomes,
                    CreatedAt = courseSession.CreatedAt,
                    UpdatedAt = courseSession.UpdatedAt
                });
            }

            await courseModuleCourseSessionRepo.FindAll(x =>
                    x.CourseModuleId == targetModuleId && x.CourseSessionId == courseSession.Id)
                .FirstOrDefaultAsync(cancellationToken);

            _logger.LogInformation("End UpdateCourseSessionCommandHandler");
            return Result.Success(new GetCourseSessionResponse
            {
                Id = courseSession.Id,
                ModuleId = targetModuleId,
                Title = courseSession.Title,
                Description = courseSession.Description,
                Outcomes = courseSession.Outcomes,
                CreatedAt = courseSession.CreatedAt,
                UpdatedAt = courseSession.UpdatedAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateCourseSessionCommandHandler: {Message}", ex.Message);
            return Result.Failure<GetCourseSessionResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
