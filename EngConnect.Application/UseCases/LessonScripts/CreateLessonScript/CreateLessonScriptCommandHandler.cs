using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.LessonScripts.CreateLessonScript;

public class CreateLessonScriptCommandHandler : ICommandHandler<CreateLessonScriptCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateLessonScriptCommandHandler> _logger;

    public CreateLessonScriptCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateLessonScriptCommandHandler> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> HandleAsync(CreateLessonScriptCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateLessonScriptCommandHandler: {@command}", command);
        try
        {
            var lessonExists = await _unitOfWork.GetRepository<Lesson, Guid>()
                .AnyAsync(x => x.Id == command.LessonId, cancellationToken: cancellationToken);

            var lessonRecord = await _unitOfWork.GetRepository<LessonRecord, Guid>()
                .FindFirstAsync(x => x.Id == command.RecordId, cancellationToken: cancellationToken);

            var lessonScriptByLessonExists = await _unitOfWork.GetRepository<LessonScript, Guid>()
                .AnyAsync(x => x.LessonId == command.LessonId, cancellationToken: cancellationToken);

            var lessonScriptByRecordExists = await _unitOfWork.GetRepository<LessonScript, Guid>()
                .AnyAsync(x => x.RecordId == command.RecordId, cancellationToken: cancellationToken);

            if (!lessonExists)
            {
                _logger.LogWarning("Lesson khÃ´ng tá»“n táº¡i: {lessonId}", command.LessonId);
                return Result.Failure<Guid>(HttpStatusCode.NotFound, CommonErrors.NotFound<Lesson>("LessonId khÃ´ng tá»“n táº¡i"));
            }

            if (lessonRecord == null)
            {
                _logger.LogWarning("LessonRecord khÃ´ng tá»“n táº¡i: {recordId}", command.RecordId);
                return Result.Failure<Guid>(HttpStatusCode.NotFound, CommonErrors.NotFound<LessonRecord>("RecordId khÃ´ng tá»“n táº¡i"));
            }

            if (lessonRecord.LessonId != command.LessonId)
            {
                _logger.LogWarning("LessonScript lesson/record mismatch. LessonId: {lessonId}, RecordId: {recordId}", command.LessonId, command.RecordId);
                return Result.Failure<Guid>(HttpStatusCode.BadRequest,
                    CommonErrors.ValidationFailed("RecordId does not belong to LessonId"));
            }

            if (lessonScriptByLessonExists)
            {
                _logger.LogWarning("LessonScript already exists for lesson: {lessonId}", command.LessonId);
                return Result.Failure<Guid>(HttpStatusCode.Conflict,
                    CommonErrors.ValidationFailed("Lesson already has a lesson script"));
            }

            if (lessonScriptByRecordExists)
            {
                _logger.LogWarning("LessonScript already exists for record: {recordId}", command.RecordId);
                return Result.Failure<Guid>(HttpStatusCode.Conflict,
                    CommonErrors.ValidationFailed("Lesson record already has a lesson script"));
            }

            var lessonScript = command.Adapt<LessonScript>();
            _unitOfWork.GetRepository<LessonScript, Guid>().Add(lessonScript);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("End CreateLessonScriptCommandHandler");

            return Result.Success(lessonScript);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateLessonScriptCommandHandler {@Message}", ex.Message);
            return Result.Failure<Guid>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
