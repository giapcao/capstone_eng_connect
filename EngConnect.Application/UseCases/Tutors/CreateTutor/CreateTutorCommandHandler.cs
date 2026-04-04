using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Tutors.CreateTutor
{
    public class CreateTutorCommandHandler : ICommandHandler<CreateTutorCommand>
    {
        private readonly ILogger<CreateTutorCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAwsStorageService _awsStorageService;

        public CreateTutorCommandHandler(
            ILogger<CreateTutorCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IAwsStorageService awsStorageService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _awsStorageService = awsStorageService;
        }

        public async Task<Result> HandleAsync(
            CreateTutorCommand command,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Start CreateTutorCommandHandler: {@Command}", command);

            try
            {
                var repo = _unitOfWork.GetRepository<Domain.Persistence.Models.Tutor, Guid>();
                string? introVideoUrl = null;
                string? cvUrl = null;

                if (command.IntroVideoFile != null)
                {
                    var introVideoResult = await _awsStorageService.UploadFileAsync(
                        command.IntroVideoFile,
                        command.UserId,
                        nameof(PrefixFile.IntroVideo),
                        cancellationToken);

                    if (introVideoResult == null)
                    {
                        _logger.LogWarning("Failed to upload intro video file for user {UserId}", command.UserId);
                        return Result.Failure(
                            HttpStatusCode.BadRequest,
                            CommonErrors.ValidationFailed("IntroVideoFile"));
                    }

                    introVideoUrl = introVideoResult.RelativePath;
                }

                if (command.CvFile != null)
                {
                    var cvResult = await _awsStorageService.UploadFileAsync(
                        command.CvFile,
                        command.UserId,
                        nameof(PrefixFile.CV),
                        cancellationToken);

                    if (cvResult == null)
                    {
                        _logger.LogWarning("Failed to upload CV file for user {UserId}", command.UserId);
                        return Result.Failure(
                            HttpStatusCode.BadRequest,
                            CommonErrors.ValidationFailed("CvFile"));
                    }

                    cvUrl = cvResult.RelativePath;
                }

                var entity = new Domain.Persistence.Models.Tutor
                {
                    UserId = command.UserId,
                    Headline = command.Headline,
                    Bio = command.Bio,
                    IntroVideoUrl = introVideoUrl,
                    MonthExperience = command.MonthExperience,
                    CvUrl = cvUrl,
                    SlotsCount = 0,
                    Status = nameof(CommonStatus.Active),
                    VerifiedStatus = nameof(TutorVerifiedStatus.Unverified)
                };

                repo.Add(entity);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("End CreateTutorCommandHandler");
                return Result.Success(entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in CreateTutorCommandHandler {@Message}", ex.Message);
                return Result.Failure<Guid>(
                    HttpStatusCode.InternalServerError,
                    CommonErrors.InternalServerError());
            }
        }
    }
}
