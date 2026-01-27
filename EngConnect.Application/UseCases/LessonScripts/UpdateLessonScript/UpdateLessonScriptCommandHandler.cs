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
            
            var lessonExists = await  _unitOfWork.GetRepository<Lesson, Guid>()
                .AnyAsync(x => x.Id == command.LessonId, cancellationToken);
            
            var lessonRecordExists = await _unitOfWork.GetRepository<LessonRecord, Guid>()
                .AnyAsync(x => x.Id == command.RecordId, cancellationToken);
            
            if (lessonScriptExists == null)
            {
                _logger.LogWarning("LessonScript không tồn tại {id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<LessonScript>("Id"));
            }
            
            command.Adapt(lessonScriptExists);
            
            if (!lessonRecordExists)
            {
                _logger.LogWarning("LessonRecord không tồn tại {RecordId}", command.RecordId);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<LessonRecord>("RecordId"));
            }

            if (!lessonExists)
            {
                _logger.LogWarning("Lesson not found {LessonId}", command.LessonId);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Lesson>("LessonId"));
            }
            
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
