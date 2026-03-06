using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseResources.CreateCourseResource;

public class CreateCourseResourceCommandHandler : ICommandHandler<CreateCourseResourceCommand>
{
    private readonly ILogger<CreateCourseResourceCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseResourceCommandHandler(ILogger<CreateCourseResourceCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateCourseResourceCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateCourseResourceCommandHandler {@Command}", command);
        try
        {
            var courseResourceRepo = _unitOfWork.GetRepository<CourseResource, Guid>();
            var courseSessionRepo = _unitOfWork.GetRepository<CourseSession, Guid>();

            // Check if session exists
            var sessionExists = await courseSessionRepo.AnyAsync(x => x.Id == command.SessionId, cancellationToken);
            if (!sessionExists)
            {
                _logger.LogWarning("Session not found with ID: {SessionId}", command.SessionId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("SessionNotFound", "Session không tồn tại"));
            }

            var courseResource = new CourseResource
            {
                SessionId = command.SessionId,
                Title = command.Title,
                ResourceType = command.ResourceType,
                Url = command.Url,
                Status = nameof(CommonStatus.Active)
            };

            courseResourceRepo.Add(courseResource);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End CreateCourseResourceCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateCourseResourceCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
