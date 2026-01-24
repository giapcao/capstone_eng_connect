using EngConnect.Application.UseCases.Tutor.Extensions;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Application.UseCases.Tutor.UpdateTutor
{
    internal class UpdateTutorCommandHandler : ICommandHandler<UpdateTutorCommand>
    {
        private readonly ILogger<UpdateTutorCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTutorCommandHandler(ILogger<UpdateTutorCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> HandleAsync(
            UpdateTutorCommand command,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Start UpdateTutorCommandHandler: {@Command}", command);

            try
            {
                // Validate status
                if (!string.IsNullOrWhiteSpace(command.Request.Status) &&
                    !TutorStatusExtensions.IsValidTutorStatus(command.Request.Status))
                {
                    return Result.Failure(HttpStatusCode.BadRequest,
                        CommonErrors.ValidationFailed($"Invalid tutor status '{command.Request.Status}'."));
                }

                // Validate verified status
                if (!string.IsNullOrWhiteSpace(command.Request.VerifiedStatus) &&
                    !TutorStatusExtensions.IsValidTutorVerifiedStatus(command.Request.VerifiedStatus))
                {
                    return Result.Failure(HttpStatusCode.BadRequest,
                        CommonErrors.ValidationFailed($"Invalid tutor verified status '{command.Request.VerifiedStatus}'."));
                }

                var repo = _unitOfWork.GetRepository<Domain.Persistence.Models.Tutor, Guid>();

                var entity = await repo
                    .FindAll()
                    .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

                if (entity is null)
                {
                    return Result.Failure<bool>(HttpStatusCode.NotFound, CommonErrors.NotFound<Domain.Persistence.Models.Tutor>("Tutor"));
                }

                entity.Headline = command.Request.Headline ?? entity.Headline;
                entity.Bio = command.Request.Bio ?? entity.Bio;
                entity.IntroVideoUrl = command.Request.IntroVideoUrl ?? entity.IntroVideoUrl;
                entity.YearsExperience = command.Request.YearsExperience ?? entity.YearsExperience;
                entity.CvUrl = command.Request.CvUrl ?? entity.CvUrl;
                entity.Tags = command.Request.Tags ?? entity.Tags;
                entity.SlotsCount = command.Request.SlotsCount ?? entity.SlotsCount;
                entity.Status = command.Request.Status ?? entity.Status;
                entity.VerifiedStatus = command.Request.VerifiedStatus ?? entity.VerifiedStatus;

                repo.Update(entity);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("End UpdateTutorCommandHandler");
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in UpdateTutorCommandHandler {@Message}", ex.Message);
                return Result.Failure<bool>(HttpStatusCode.InternalServerError,
                    CommonErrors.InternalServerError());
            }
        }
    }
}