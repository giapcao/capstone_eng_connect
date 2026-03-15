using System.Net;
using EngConnect.Application.UseCases.Tutors.Common;
using EngConnect.Application.UseCases.Users.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Tutors.GetTutorById
{
    public class GetTutorByIdQueryHandler : IQueryHandler<GetTutorByIdQuery, GetTutorResponse>
    {
        private readonly ILogger<GetTutorByIdQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAwsStorageService _awsStorageService;

        public GetTutorByIdQueryHandler(ILogger<GetTutorByIdQueryHandler> logger, IUnitOfWork unitOfWork,
            IAwsStorageService awsStorageService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _awsStorageService = awsStorageService;
        }

        public async Task<Result<GetTutorResponse>> HandleAsync(
            GetTutorByIdQuery query,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Start GetTutorByIdQueryHandler: {@Query}", query);

            try
            {
                var repo = _unitOfWork.GetRepository<Domain.Persistence.Models.Tutor, Guid>();

                var tutor = await repo
                    .FindAll()
                    .Where(x => x.Id == query.Id)
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(cancellationToken);

                if (tutor is null)
                {
                    return Result.Failure<GetTutorResponse>(HttpStatusCode.NotFound, CommonErrors.NotFound<Domain.Persistence.Models.Tutor>("Tutor"));
                }

                var response = new GetTutorResponse
                {
                    Id = tutor.Id,
                    UserId = tutor.UserId,
                    Headline = tutor.Headline,
                    Bio = tutor.Bio,
                    IntroVideoUrl = tutor.IntroVideoUrl,
                    YearsExperience = tutor.YearsExperience,
                    CvUrl = tutor.CvUrl,
                    Tags = tutor.Tags,
                    SlotsCount = tutor.SlotsCount,
                    Status = tutor.Status,
                    VerifiedStatus = tutor.VerifiedStatus,
                    RatingAverage = tutor.RatingAverage,
                    RatingCount = tutor.RatingCount,
                    CreatedAt = tutor.CreatedAt,
                    UpdatedAt = tutor.UpdatedAt,
                    IsDeleted = tutor.IsDeleted,
                    DeletedAt = tutor.DeletedAt,
                    User = new UserInfo
                    {
                        FirstName = tutor.User.FirstName,
                        LastName = tutor.User.LastName,
                        UserName = tutor.User.UserName,
                        Email = tutor.User.Email,
                        Phone = tutor.User.Phone,
                        AvatarUrl = _awsStorageService.GetFileUrl(tutor.Avatar)
                    }
                };

                _logger.LogInformation("End GetTutorByIdQueryHandler");
                return Result.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in GetTutorByIdQueryHandler {@Message}", ex.Message);
                return Result.Failure<GetTutorResponse>(HttpStatusCode.InternalServerError,
                    CommonErrors.InternalServerError());
            }
        }
    }
}