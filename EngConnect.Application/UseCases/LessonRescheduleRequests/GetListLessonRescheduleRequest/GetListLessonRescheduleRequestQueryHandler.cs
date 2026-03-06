using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.LessonRescheduleRequests.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.LessonRescheduleRequests.GetListLessonRescheduleRequest;

public sealed class GetListLessonRescheduleRequestQueryHandler
    : IQueryHandler<GetListLessonRescheduleRequestQuery, PaginationResult<GetRescheduleRequestResponse>>
{
    private readonly ILogger<GetListLessonRescheduleRequestQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListLessonRescheduleRequestQueryHandler(
        ILogger<GetListLessonRescheduleRequestQueryHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetRescheduleRequestResponse>>> HandleAsync(
        GetListLessonRescheduleRequestQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListLessonRescheduleRequestQueryHandler {@Query}", query);

        try
        {
            var requests = _unitOfWork.GetRepository<LessonRescheduleRequest, Guid>()
                .FindAll();

            Expression<Func<LessonRescheduleRequest, bool>>? predicate = x => true;

            if (query.LessonId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.LessonId == query.LessonId.Value);
            }

            if (query.StudentId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.StudentId == query.StudentId.Value);
            }

            if (ValidationUtil.IsNotNullOrEmpty(query.Status))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Status == query.Status);
            }

            requests = requests.Where(predicate);

            requests = requests
                .ApplySearch(query.GetSearchParams(),
                    x => x.Status!,
                    x => x.TutorNote!)
                .ApplySorting(query.GetSortParams());

            var result = await requests.ProjectToPaginatedListAsync<LessonRescheduleRequest, GetRescheduleRequestResponse>(
                query.GetPaginationParams());

            _logger.LogInformation("End GetListLessonRescheduleRequestQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListLessonRescheduleRequestQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetRescheduleRequestResponse>>(
                HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}