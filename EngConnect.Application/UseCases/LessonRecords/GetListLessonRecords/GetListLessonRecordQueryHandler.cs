using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.LessonRecords.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.LessonRecords.GetListLessonRecords;

public class GetListLessonRecordQueryHandler : IQueryHandler<GetListLessonRecordQuery, PaginationResult<GetLessonRecordResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetListLessonRecordQueryHandler> _logger;

    public GetListLessonRecordQueryHandler(ILogger<GetListLessonRecordQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetLessonRecordResponse>>> HandleAsync(GetListLessonRecordQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListLessonRecordQueryHandler: {@query}", query);
        try
        {
            var lessonRecords = _unitOfWork.GetRepository<LessonRecord, Guid>().FindAll();
            
            if(query.LessonId.HasValue)
                lessonRecords = lessonRecords.Where(x=>x.LessonId == query.LessonId);
            
            lessonRecords = lessonRecords.ApplySorting(query.GetSortParams());

            var result = await lessonRecords.ProjectToPaginatedListAsync<LessonRecord, GetLessonRecordResponse>
                (query.GetPaginationParams());

            _logger.LogInformation("End GetListLessonRecordQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListLessonRecordQueryHandler: {@Message}", ex.Message);
            return Result.Failure<PaginationResult<GetLessonRecordResponse>>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
