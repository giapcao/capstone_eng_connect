using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript;

public class UpdateLessonScriptCommandHandler : ICommandHandler<UpdateLessonScriptCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateLessonScriptCommandHandler> _logger;

    public UpdateLessonScriptCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateLessonScriptCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Result> HandleAsync(UpdateLessonScriptCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateLessonScriptCommandHandler: {@command}", command);
        try
        {
            var lessonScriptExists = await _unitOfWork.GetRepository<LessonScript, Guid>()
                .FindByIdAsync(command.Id, cancellationToken: cancellationToken);

            if (lessonScriptExists == null)
            {
                _logger.LogWarning("LessonScript khÃ´ng tá»“n táº¡i {id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<LessonScript>("Id"));
            }

            var lessonExists = await _unitOfWork.GetRepository<Lesson, Guid>()
                .AnyAsync(x => x.Id == command.LessonId, cancellationToken);

            var lessonRecord = await _unitOfWork.GetRepository<LessonRecord, Guid>()
                .FindFirstAsync(x => x.Id == command.RecordId, cancellationToken: cancellationToken);

            if (lessonRecord == null)
            {
                _logger.LogWarning("LessonRecord khÃ´ng tá»“n táº¡i {RecordId}", command.RecordId);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<LessonRecord>("RecordId"));
            }

            if (!lessonExists)
            {
                _logger.LogWarning("Lesson not found {LessonId}", command.LessonId);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Lesson>("LessonId"));
            }

            if (lessonRecord.LessonId != command.LessonId)
            {
                _logger.LogWarning("LessonScript lesson/record mismatch. LessonId: {lessonId}, RecordId: {recordId}", command.LessonId, command.RecordId);
                return Result.Failure(HttpStatusCode.BadRequest,
                    CommonErrors.ValidationFailed("RecordId does not belong to LessonId"));
            }

            var lessonScriptByLessonExists = await _unitOfWork.GetRepository<LessonScript, Guid>()
                .AnyAsync(x => x.LessonId == command.LessonId && x.Id != command.Id, cancellationToken: cancellationToken);

            if (lessonScriptByLessonExists)
            {
                _logger.LogWarning("Another LessonScript already exists for lesson: {lessonId}", command.LessonId);
                return Result.Failure(HttpStatusCode.Conflict,
                    CommonErrors.ValidationFailed("Lesson already has a lesson script"));
            }

            var lessonScriptByRecordExists = await _unitOfWork.GetRepository<LessonScript, Guid>()
                .AnyAsync(x => x.RecordId == command.RecordId && x.Id != command.Id, cancellationToken: cancellationToken);

            if (lessonScriptByRecordExists)
            {
                _logger.LogWarning("Another LessonScript already exists for record: {recordId}", command.RecordId);
                return Result.Failure(HttpStatusCode.Conflict,
                    CommonErrors.ValidationFailed("Lesson record already has a lesson script"));
            }

            command.Adapt(lessonScriptExists);

            _unitOfWork.GetRepository<LessonScript, Guid>().Update(lessonScriptExists);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("End UpdateLessonScriptCommandHandler");
            return Result.Success(lessonScriptExists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateLessonScriptCommandHandler {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
