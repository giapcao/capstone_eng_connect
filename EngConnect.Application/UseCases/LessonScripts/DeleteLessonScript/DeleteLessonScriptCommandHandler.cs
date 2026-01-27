using System.Net;
using EngConnect.Application.UseCases.LessonScripts.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.LessonScripts.DeleteLessonScript;

public class DeleteLessonScriptCommandHandler : ICommandHandler<DeleteLessonScriptCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteLessonScriptCommandHandler> _logger;

    public DeleteLessonScriptCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteLessonScriptCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Result> HandleAsync(DeleteLessonScriptCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteLessonScriptCommandHandler : {@command}", command);
        try
        {
            var lessonScriptExists = await _unitOfWork.GetRepository<LessonScript, Guid>()
                .FindByIdAsync(command.Id, cancellationToken: cancellationToken);
            
            if (lessonScriptExists == null)
            {
                _logger.LogWarning("LessonScript không tồn tại: {lessonScriptId}", command.Id);
                return Result.Failure<GetLessonScriptResponse>(HttpStatusCode.NotFound,
                    CommonErrors.NotFound<LessonScript>("Id"));
            }
            
            _unitOfWork.GetRepository<LessonScript, Guid>().Delete(lessonScriptExists);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("End DeleteLessonScriptCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteLessonScriptCommandHandler {@Message}", ex.Message);
            return Result.Failure<GetLessonScriptResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
