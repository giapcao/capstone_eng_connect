using EngConnect.Application.UseCases.TutorVerification.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Net;

namespace EngConnect.Application.UseCases.TutorVerification.GetListTutorVerificationRequest
{
    public class GetListTutorVerificationRequestQueryHandler
    : IQueryHandler<GetListTutorVerificationRequestQuery, PaginationResult<GetTutorVerificationRequestResponse>>
    {
        private readonly ILogger<GetListTutorVerificationRequestQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public GetListTutorVerificationRequestQueryHandler(
            ILogger<GetListTutorVerificationRequestQueryHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginationResult<GetTutorVerificationRequestResponse>>> HandleAsync(
            GetListTutorVerificationRequestQuery query,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Start GetListTutorVerificationRequestQueryHandler {@Query}", query);

            try
            {
                var tutorVerificationRequests = _unitOfWork
                    .GetRepository<TutorVerificationRequest, Guid>()
                    .FindAll();

                Expression<Func<TutorVerificationRequest, bool>> predicate = _ => true;

                if (query.TutorId.HasValue)
                {
                    predicate = predicate.CombineAndAlsoExpressions(x => x.TutorId == query.TutorId.Value);
                }

                if (ValidationUtil.IsNotNullOrEmpty(query.Status))
                {
                    predicate = predicate.CombineAndAlsoExpressions(x => x.Status == query.Status);
                }

                if (query.ReviewedBy.HasValue)
                {
                    predicate = predicate.CombineAndAlsoExpressions(x => x.ReviewedBy == query.ReviewedBy.Value);
                }

                tutorVerificationRequests = tutorVerificationRequests.Where(predicate);

                tutorVerificationRequests = tutorVerificationRequests
                    .ApplySearch(
                        query.GetSearchParams(),
                        x => x.Status,
                        x => x.RejectionReason)
                    .ApplySorting(query.GetSortParams());

                var result = await tutorVerificationRequests
                    .ProjectToPaginatedListAsync<TutorVerificationRequest, GetTutorVerificationRequestResponse>(
                        query.GetPaginationParams());

                _logger.LogInformation("End GetListTutorVerificationRequestQueryHandler");

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetListTutorVerificationRequestQueryHandler: {Message}", ex.Message);

                return Result.Failure<PaginationResult<GetTutorVerificationRequestResponse>>(
                    HttpStatusCode.InternalServerError,
                    CommonErrors.InternalServerError());
            }
        }
    }
}