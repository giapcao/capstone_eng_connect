using System.Net;
using EngConnect.Application.UseCases.LessonRecords.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord;

public class CreateLessonRecordCommandHandler : ICommandHandler<CreateLessonRecordCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateLessonRecordCommandHandler> _logger;

    public CreateLessonRecordCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateLessonRecordCommandHandler> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateLessonRecordCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateLessonRecordCommandHandler: {@command}", command);
        try
        {
            var lessonExists = await _unitOfWork.GetRepository<Lesson, Guid>()
                .AnyAsync(x => x.Id == command.LessonId, cancellationToken: cancellationToken);

            if (!lessonExists)
            {
                _logger.LogWarning("Lesson not found: {lessonId}", command.LessonId);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Lesson>("Bài học"));
            }

            var lessonRecord = command.Adapt<LessonRecord>();
            _unitOfWork.GetRepository<LessonRecord, Guid>().Add(lessonRecord);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End CreateLessonRecordCommandHandler");
            return Result.Success(lessonRecord);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateLessonRecordCommandHandler: {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
