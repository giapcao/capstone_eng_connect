using System.Net;
using EngConnect.Application.UseCases.TutorSchedules.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.TutorSchedules.GetTutorScheduleById;

public sealed class GetTutorScheduleByIdQueryHandler
    : IQueryHandler<GetTutorScheduleByIdQuery, GetTutorScheduleResponse>
{
    private readonly ILogger<GetTutorScheduleByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetTutorScheduleByIdQueryHandler(
        ILogger<GetTutorScheduleByIdQueryHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetTutorScheduleResponse>> HandleAsync(
        GetTutorScheduleByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetTutorScheduleByIdQueryHandler {@Query}", query);

        try
        {
            var entity = await _unitOfWork.GetRepository<TutorSchedule, Guid>()
                .FindByIdAsync(query.Id, tracking: false, cancellationToken: cancellationToken);

            if (entity is null)
            {
                return Result.Failure<GetTutorScheduleResponse>(HttpStatusCode.NotFound, ScheduleErrors.TutorScheduleNotFound());
            }

            var response = entity.Adapt<GetTutorScheduleResponse>();

            _logger.LogInformation("End GetTutorScheduleByIdQueryHandler");
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetTutorScheduleByIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetTutorScheduleResponse>(
                HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}