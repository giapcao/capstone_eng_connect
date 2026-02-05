using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.LessonScripts.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;
using EngConnect.Domain.Persistence.Models;

namespace EngConnect.Application.UseCases.LessonScripts.GetListLessonScripts;

public class GetListLessonScriptsQueryHandler : IQueryHandler<GetListLessonScriptsQuery, PaginationResult<GetLessonScriptResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetListLessonScriptsQueryHandler> _logger;

    public GetListLessonScriptsQueryHandler(ILogger<GetListLessonScriptsQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<PaginationResult<GetLessonScriptResponse>>> HandleAsync(GetListLessonScriptsQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListLessonScriptsQueryHandler : {@query}", query);
        try
        {
            var lessonScripts = _unitOfWork.GetRepository<LessonScript, Guid>().FindAll();
            Expression<Func<LessonScript, bool>> predicate = x => true;
            
            if (query.LessonId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.LessonId == query.LessonId);
            }
            
            if (query.RecordId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.RecordId == query.RecordId);
            }

            if (ValidationUtil.IsNotNullOrEmpty(query.Language))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Language != null && query.Language.ToLower().Contains(x.Language.ToLower()));
            }

            lessonScripts = lessonScripts.Where(predicate);

            lessonScripts = lessonScripts.ApplySearch(query.GetSearchParams(),
                    x => x.Language ?? string.Empty,
                    x => x.LessonOutcome ?? string.Empty,
                    x => x.SummarizeText ?? string.Empty)
                .ApplySorting(query.GetSortParams());

            var result =
                await lessonScripts.ProjectToPaginatedListAsync<LessonScript, GetLessonScriptResponse>
                    (query.GetPaginationParams());
            _logger.LogInformation("End GetListLessonScriptsQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListLessonScriptsQueryHandler {@Message}", ex.Message);
            return Result.Failure<PaginationResult<GetLessonScriptResponse>>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
