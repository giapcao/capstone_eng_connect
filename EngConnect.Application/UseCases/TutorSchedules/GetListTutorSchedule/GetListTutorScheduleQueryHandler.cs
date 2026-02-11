using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.TutorSchedules.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.TutorSchedules.GetListTutorSchedule;

public sealed class GetListTutorScheduleQueryHandler
    : IQueryHandler<GetListTutorScheduleQuery, PaginationResult<GetTutorScheduleResponse>>
{
    private readonly ILogger<GetListTutorScheduleQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListTutorScheduleQueryHandler(
        ILogger<GetListTutorScheduleQueryHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetTutorScheduleResponse>>> HandleAsync(
        GetListTutorScheduleQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListTutorScheduleQueryHandler {@Query}", query);

        try
        {
            var tutorSchedules = _unitOfWork.GetRepository<TutorSchedule, Guid>()
                .FindAll();

            Expression<Func<TutorSchedule, bool>>? predicate = x => true;

            if (query.TutorId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.TutorId == query.TutorId.Value);
            }

            if (ValidationUtil.IsNotNullOrEmpty(query.Weekday))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Weekday == query.Weekday);
            }

            tutorSchedules = tutorSchedules.Where(predicate);

            tutorSchedules = tutorSchedules
                .ApplySearch(query.GetSearchParams(),
                    x => x.Weekday!)
                .ApplySorting(query.GetSortParams());

            var result = await tutorSchedules
                .ProjectToPaginatedListAsync<TutorSchedule, GetTutorScheduleResponse>(query.GetPaginationParams());

            _logger.LogInformation("End GetListTutorScheduleQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListTutorScheduleQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetTutorScheduleResponse>>(
                HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}