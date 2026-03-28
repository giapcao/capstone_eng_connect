using System.Net;
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

public class UpdateCourseSessionCommandHandler : ICommandHandler<UpdateCourseSessionCommand>
{
    private readonly ILogger<UpdateCourseSessionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseSessionCommandHandler(ILogger<UpdateCourseSessionCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateCourseSessionCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateCourseSessionCommandHandler {@Command}", command);
        try
        {
            var courseSessionRepo = _unitOfWork.GetRepository<CourseSession, Guid>();

            var courseSession = await courseSessionRepo.FindSingleAsync(
                x => x.Id == command.Id,
                cancellationToken: cancellationToken);
            if (courseSession == null)
            {
                _logger.LogWarning("CourseSession not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseSessionNotFound", "Session không tồn tại"));
            }

            // Check status of courses that use this session
            var listCourse = await _unitOfWork.GetRepository<CourseModuleCourseSession, Guid>()
                .FindAll(x => x.CourseSessionId == command.Id)
                .Include(x => x.CourseModule)
                    .ThenInclude(cm => cm.CourseCourseModules)
                        .ThenInclude(ccm => ccm.Course)
                .ToListAsync(cancellationToken);

            var hasPublishedCourse = listCourse
                .SelectMany(x => x.CourseModule.CourseCourseModules)
                .Any(ccm => ccm.Course.Status == nameof(CourseStatus.Published));

            if (hasPublishedCourse)
            {
                _logger.LogWarning("CourseSession with ID: {Id} cannot be updated because it's in use by a Published course", command.Id);
                return Result.Failure(HttpStatusCode.BadRequest, CourseSessionErrors.CourseSessionIsInUse());
            }

            courseSession.Title = command.Title;
            courseSession.Description = command.Description;
            courseSession.Outcomes = command.Outcomes;

            courseSessionRepo.Update(courseSession);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdateCourseSessionCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateCourseSessionCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
