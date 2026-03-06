using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.CourseVerificationRequests.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseVerificationRequests.GetListCourseVerificationRequest;

public class GetListCourseVerificationRequestQueryHandler : IQueryHandler<GetListCourseVerificationRequestQuery, PaginationResult<GetCourseVerificationRequestResponse>>
{
    private readonly ILogger<GetListCourseVerificationRequestQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListCourseVerificationRequestQueryHandler(ILogger<GetListCourseVerificationRequestQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetCourseVerificationRequestResponse>>> HandleAsync(GetListCourseVerificationRequestQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListCourseVerificationRequestQueryHandler {@Query}", query);
        try
        {
            var courseVerificationRequests = _unitOfWork.GetRepository<CourseVerificationRequest, Guid>()
                .FindAll();

            Expression<Func<CourseVerificationRequest, bool>>? predicate = x => true;

            // Apply filters
            if (query.CourseId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.CourseId == query.CourseId.Value);
            }

            if (ValidationUtil.IsNotNullOrEmpty(query.Status))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Status == query.Status);
            }

            if (query.ReviewedBy.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.ReviewedBy == query.ReviewedBy.Value);
            }

            courseVerificationRequests = courseVerificationRequests.Where(predicate);

            // Apply search and sort
            courseVerificationRequests = courseVerificationRequests.ApplySearch(query.GetSearchParams(),
                    x => x.Status,
                    x => x.RejectionReason)
                .ApplySorting(query.GetSortParams());

            // Map to GetCourseVerificationRequestResponse
            var result =
                await courseVerificationRequests.ProjectToPaginatedListAsync<CourseVerificationRequest, GetCourseVerificationRequestResponse>(
                    query.GetPaginationParams());

            _logger.LogInformation("End GetListCourseVerificationRequestQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListCourseVerificationRequestQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetCourseVerificationRequestResponse>>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
