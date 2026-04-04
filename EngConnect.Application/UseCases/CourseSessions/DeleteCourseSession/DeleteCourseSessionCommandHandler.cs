using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseSessions.DeleteCourseSession;

public class DeleteCourseSessionCommandHandler : ICommandHandler<DeleteCourseSessionCommand>
{
    private readonly ILogger<DeleteCourseSessionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourseSessionCommandHandler(ILogger<DeleteCourseSessionCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeleteCourseSessionCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteCourseSessionCommandHandler {@Command}", command);
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
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseSessionNotFound", "Session không tồn tại"));
            }

            var relations = await courseModuleCourseSessionRepo.FindAll(x => x.CourseSessionId == command.Id)
                .ToListAsync(cancellationToken);

            foreach (var relation in relations)
            {
                courseModuleCourseSessionRepo.Delete(relation);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End DeleteCourseSessionCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteCourseSessionCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
