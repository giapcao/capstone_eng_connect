using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
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

    public async Task<Result> HandleAsync(CreateCourseSessionCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateCourseSessionCommandHandler {@Command}", command);
        try
        {
            var courseSessionRepo = _unitOfWork.GetRepository<CourseSession, Guid>();
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();

            // Check if module exists
            var moduleExists = await courseModuleRepo.AnyAsync(x => x.Id == command.ModuleId, cancellationToken);
            if (!moduleExists)
            {
                _logger.LogWarning("Module not found with ID: {ModuleId}", command.ModuleId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("ModuleNotFound", "Module không tồn tại"));
            }

            var courseSession = new CourseSession
            {
                ModuleId = command.ModuleId,
                Title = command.Title,
                Description = command.Description,
                Outcomes = command.Outcomes,
                SessionNumber = command.SessionNumber
            };

            courseSessionRepo.Add(courseSession);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End CreateCourseSessionCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateCourseSessionCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
