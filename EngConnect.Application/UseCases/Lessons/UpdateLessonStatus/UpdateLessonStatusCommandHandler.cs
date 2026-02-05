using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Lessons.UpdateLessonStatus;

public class UpdateLessonStatusCommandHandler : ICommandHandler<UpdateLessonStatusCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateLessonStatusCommandHandler> _logger;

    public UpdateLessonStatusCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateLessonStatusCommandHandler> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateLessonStatusCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateLessonStatusCommandHandler: {@command}", command);
        try
        {
            var lesson = await _unitOfWork.GetRepository<Lesson, Guid>()
                .FindByIdAsync(command.Id, cancellationToken: cancellationToken);

            if (lesson == null)
            {
                _logger.LogWarning("Lesson not found: {id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Lesson>("Bài học"));
            }

            lesson.Status = command.Status;
            
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdateLessonStatusCommandHandler - Lesson {id} status updated to {status}", 
                command.Id, command.Status);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateLessonStatusCommandHandler: {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
