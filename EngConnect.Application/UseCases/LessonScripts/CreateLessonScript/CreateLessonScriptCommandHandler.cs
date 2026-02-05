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
            
            var recordExists = await _unitOfWork.GetRepository<LessonRecord, Guid>()
                .AnyAsync(x => x.Id == command.RecordId, cancellationToken: cancellationToken);
            
            if (!lessonExists)
            {
                _logger.LogWarning("Lesson không tồn tại: {lessonId}", command.LessonId);
                return Result.Failure<Guid>(HttpStatusCode.NotFound, CommonErrors.NotFound<Lesson>("LessonId không tồn tại"));
            }
            
            if (!recordExists)
            {
                _logger.LogWarning("LessonRecord không tồn tại: {recordId}", command.RecordId);
                return Result.Failure<Guid>(HttpStatusCode.NotFound, CommonErrors.NotFound<LessonRecord>("RecordId không tồn tại"));
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
