using System.Net;
using EngConnect.Application.UseCases.LessonRecords.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.LessonRecords.GetLessonRecordById;

public class GetLessonRecordByIdQueryHandler : IQueryHandler<GetLessonRecordByIdQuery, GetLessonRecordResponse>
{
    private readonly ILogger<GetLessonRecordByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetLessonRecordByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetLessonRecordByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<GetLessonRecordResponse>> HandleAsync(GetLessonRecordByIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetLessonRecordByIdQueryHandler: {@query}", query);
        try
        {
            var lessonRecord = await _unitOfWork.GetRepository<LessonRecord, Guid>()
                .FindByIdAsync(query.Id, true, cancellationToken, x => x.LessonScript!);

            if (lessonRecord == null)
            {
                _logger.LogWarning("LessonRecord not found: {id}", query.Id);
                return Result.Failure<GetLessonRecordResponse>(HttpStatusCode.NotFound,
                    CommonErrors.NotFound<LessonRecord>("Bản ghi bài học"));
            }

            var response = lessonRecord.Adapt<GetLessonRecordResponse>();
            _logger.LogInformation("End GetLessonRecordByIdQueryHandler");
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetLessonRecordByIdQueryHandler: {@Message}", ex.Message);
            return Result.Failure<GetLessonRecordResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
