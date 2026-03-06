using System.Net;
using EngConnect.Application.UseCases.LessonRescheduleRequests.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.LessonRescheduleRequests.GetLessonRescheduleRequestById;

public sealed class GetLessonRescheduleRequestByIdQueryHandler : IQueryHandler<GetLessonRescheduleRequestByIdQuery, GetRescheduleRequestResponse>
{
    private readonly ILogger<GetLessonRescheduleRequestByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetLessonRescheduleRequestByIdQueryHandler(
        ILogger<GetLessonRescheduleRequestByIdQueryHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetRescheduleRequestResponse>> HandleAsync(
        GetLessonRescheduleRequestByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetLessonRescheduleRequestByIdQueryHandler {@Query}", query);

        try
        {
            var entity = await _unitOfWork.GetRepository<LessonRescheduleRequest, Guid>()
                .FindByIdAsync(query.Id, tracking: false, cancellationToken: cancellationToken);

            if (entity is null)
            {
                return Result.Failure<GetRescheduleRequestResponse>(HttpStatusCode.NotFound, ScheduleErrors.RescheduleRequestNotFound());
            }

            var response = entity.Adapt<GetRescheduleRequestResponse>();

            _logger.LogInformation("End GetLessonRescheduleRequestByIdQueryHandler");
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetLessonRescheduleRequestByIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetRescheduleRequestResponse>(
                HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}