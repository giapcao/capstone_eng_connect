using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.LessonRecords.UpdateLessonRecord;

public class UpdateLessonRecordCommandHandler : ICommandHandler<UpdateLessonRecordCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateLessonRecordCommandHandler> _logger;

    public UpdateLessonRecordCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateLessonRecordCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(UpdateLessonRecordCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateLessonRecordCommandHandler: {@command}", command);
        try
        {
            var lessonRecord = await _unitOfWork.GetRepository<LessonRecord, Guid>()
                .FindByIdAsync(command.Id, cancellationToken: cancellationToken);

            if (lessonRecord == null)
            {
                _logger.LogWarning("LessonRecord not found: {id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<LessonRecord>("Bản ghi bài học"));
            }

            var lessonExists = await _unitOfWork.GetRepository<Lesson, Guid>()
                .AnyAsync(x => x.Id == command.LessonId, cancellationToken: cancellationToken);

            if (!lessonExists)
            {
                _logger.LogWarning("Lesson not found: {lessonId}", command.LessonId);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Lesson>("Bài học"));
            }

            command.Adapt(lessonRecord);
            _unitOfWork.GetRepository<LessonRecord, Guid>().Update(lessonRecord);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdateLessonRecordCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateLessonRecordCommandHandler: {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
