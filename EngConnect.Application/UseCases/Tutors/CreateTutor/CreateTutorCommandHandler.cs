using System.Net;
using EngConnect.Application.UseCases.Tutors.Extensions;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Tutors.CreateTutor
{
    public class CreateTutorCommandHandler : ICommandHandler<CreateTutorCommand>
    {
        private readonly ILogger<CreateTutorCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTutorCommandHandler(ILogger<CreateTutorCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            CreateTutorCommand command,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Start CreateTutorCommandHandler: {@Command}", command);

            try
            {
                // Validate status
                if (!string.IsNullOrWhiteSpace(command.Request.Status) &&
                    !TutorStatusExtensions.IsValidTutorStatus(command.Request.Status))
                {
                    return Result.Failure(HttpStatusCode.BadRequest,
                        TutorErrors.InvalidStatus(command.Request.Status));
                }

                // Validate verified status
                if (!string.IsNullOrWhiteSpace(command.Request.VerifiedStatus) &&
                    !TutorStatusExtensions.IsValidTutorVerifiedStatus(command.Request.VerifiedStatus))
                {
                    return Result.Failure(HttpStatusCode.BadRequest,
                        TutorErrors.InvalidVerifiedStatus(command.Request.VerifiedStatus));
                }

                var repo = _unitOfWork.GetRepository<Domain.Persistence.Models.Tutor, Guid>();

                var entity = new Domain.Persistence.Models.Tutor
                {
                    UserId = command.Request.UserId,
                    Headline = command.Request.Headline,
                    Bio = command.Request.Bio,
                    IntroVideoUrl = command.Request.IntroVideoUrl,
                    YearsExperience = command.Request.YearsExperience,
                    CvUrl = command.Request.CvUrl,
                    Tags = command.Request.Tags,
                    SlotsCount = command.Request.SlotsCount,
                    Status = command.Request.Status,
                    VerifiedStatus = command.Request.VerifiedStatus
                };

                repo.Add(entity);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("End CreateTutorCommandHandler");
                return Result.Success(entity.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in CreateTutorCommandHandler {@Message}", ex.Message);
                return Result.Failure<Guid>(HttpStatusCode.InternalServerError,
                    CommonErrors.InternalServerError());
            }
        }
    }
}