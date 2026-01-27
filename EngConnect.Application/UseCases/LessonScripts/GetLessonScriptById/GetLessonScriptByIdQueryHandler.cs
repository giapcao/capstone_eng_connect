using System.Net;
using EngConnect.Application.UseCases.LessonScripts.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.LessonScripts.GetLessonScriptById;

public class GetLessonScriptByIdQueryHandler : IQueryHandler<GetLessonScriptByIdQuery, GetLessonScriptResponse>
{
    private readonly ILogger<GetLessonScriptByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetLessonScriptByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetLessonScriptByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Result<GetLessonScriptResponse>> HandleAsync(GetLessonScriptByIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetLessonScriptByIdQueryHandler : {@query}", query);
        try
        {
            var lessonScript = await _unitOfWork.GetRepository<LessonScript, Guid>()
                .FindByIdAsync(query.Id, cancellationToken: cancellationToken);
            
            if (lessonScript == null)
            {
                _logger.LogWarning("LessonScript không tồn tại {lessonScriptId}", query.Id);
                return Result.Failure<GetLessonScriptResponse>(HttpStatusCode.NotFound,
                    CommonErrors.NotFound<LessonScript>("Id"));
            }
            
            var response = lessonScript.Adapt<GetLessonScriptResponse>();

            _logger.LogInformation("End GetLessonScriptByIdQueryHandler");
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetLessonScriptByIdQueryHandler {@Message}", ex.Message);
            return Result.Failure<GetLessonScriptResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
