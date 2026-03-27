using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.Tutors.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Tutors.GetListTutor
{
    public class GetListTutorQueryHandler : IQueryHandler<GetListTutorQuery, PaginationResult<GetTutorResponse>>
    {
        private readonly ILogger<GetListTutorQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAwsStorageService _awsStorageService;

        public GetListTutorQueryHandler(ILogger<GetListTutorQueryHandler> logger, IUnitOfWork unitOfWork, IAwsStorageService awsStorageService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _awsStorageService = awsStorageService;
        }

        public async Task<Result<PaginationResult<GetTutorResponse>>> HandleAsync(
            GetListTutorQuery query,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Start GetListTutorQueryHandler: {@Query}", query);

            try
            {
                var tutors = _unitOfWork.GetRepository<Domain.Persistence.Models.Tutor, Guid>().FindAll();

                Expression<Func<Domain.Persistence.Models.Tutor, bool>>? predicate = x => true;

                if (ValidationUtil.IsNotNullOrEmpty(query.Status))
                {
                    predicate = predicate.CombineAndAlsoExpressions(x => query.Status.Contains(x.Status));
                }

                if (ValidationUtil.IsNotNullOrEmpty(query.VerifiedStatus))
                {
                    predicate = predicate.CombineAndAlsoExpressions(x => query.VerifiedStatus.Contains(x.VerifiedStatus));
                }

                tutors = tutors.Where(predicate);

                tutors = tutors.ApplySearch(
                        query.GetSearchParams(),
                        x => x.Headline ?? string.Empty,
                        x => x.Bio ?? string.Empty)
                    .ApplySorting(query.GetSortParams());

                var result =
                    await tutors.ProjectToPaginatedListAsync<Domain.Persistence.Models.Tutor, GetTutorResponse>(
                        query.GetPaginationParams());

                // Convert relative paths to full AWS S3 URLs
                foreach (var item in result.Items)
                {
                    item.Avatar = _awsStorageService.GetFileUrl(item.Avatar);
                    item.IntroVideoUrl = _awsStorageService.GetFileUrl(item.IntroVideoUrl);
                    item.CvUrl = _awsStorageService.GetFileUrl(item.CvUrl);
                }

                _logger.LogInformation("End GetListTutorQueryHandler");
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetListTutorQueryHandler {@Message}", ex.Message);
                return Result.Failure<PaginationResult<GetTutorResponse>>(HttpStatusCode.InternalServerError,
                    CommonErrors.InternalServerError());
            }
        }
    }
}