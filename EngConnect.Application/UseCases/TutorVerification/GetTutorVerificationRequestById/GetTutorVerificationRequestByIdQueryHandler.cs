using EngConnect.Application.UseCases.TutorVerification.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

using System.Net;

namespace EngConnect.Application.UseCases.TutorVerification.GetTutorVerificationRequestById
{
    public class GetTutorVerificationRequestByIdQueryHandler
    : IQueryHandler<GetTutorVerificationRequestByIdQuery, GetTutorVerificationRequestResponse>
    {
        private readonly ILogger<GetTutorVerificationRequestByIdQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public GetTutorVerificationRequestByIdQueryHandler(
            ILogger<GetTutorVerificationRequestByIdQueryHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetTutorVerificationRequestResponse>> HandleAsync(
            GetTutorVerificationRequestByIdQuery query,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Start GetTutorVerificationRequestByIdQueryHandler: {@Query}", query);

            try
            {
                var repo = _unitOfWork.GetRepository<TutorVerificationRequest, Guid>();

                var entity = await repo.FindByIdAsync(query.RequestId, tracking: false, cancellationToken: cancellationToken);

                if (entity is null)
                {
                    return Result.Failure<GetTutorVerificationRequestResponse>(
                        HttpStatusCode.NotFound,
                        TutorErrors.VerificationRequestNotFound(query.RequestId));
                }

                var response = new GetTutorVerificationRequestResponse
                {
                    Id = entity.Id,
                    TutorId = entity.TutorId,
                    Status = entity.Status,
                    SubmittedAt = entity.SubmittedAt,
                    ReviewedAt = entity.ReviewedAt,
                    ReviewedBy = entity.ReviewedBy,
                    RejectionReason = entity.RejectionReason
                };

                return Result.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetTutorVerificationRequestByIdQueryHandler {@Message}", ex.Message);

                return Result.Failure<GetTutorVerificationRequestResponse>(
                    HttpStatusCode.InternalServerError,
                    CommonErrors.InternalServerError());
            }
        }
    }
}