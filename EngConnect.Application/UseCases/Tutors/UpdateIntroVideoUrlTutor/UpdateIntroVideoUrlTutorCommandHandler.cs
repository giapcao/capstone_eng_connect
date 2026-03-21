using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EngConnect.Application.UseCases.Tutors.UpdateIntroVideoUrlTutor;

public class UpdateIntroVideoUrlTutorCommandHandler : ICommandHandler<UpdateIntroVideoUrlTutorCommand>
{
    private readonly ILogger<UpdateIntroVideoUrlTutorCommandHandler> _logger;
    private readonly IAwsStorageService _awsStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _cache;
    private readonly RedisCacheSettings _settings;

    public UpdateIntroVideoUrlTutorCommandHandler(IAwsStorageService awsStorageService,
        IOptions<RedisCacheSettings> settings, IRedisService cache,
        IUnitOfWork unitOfWork, ILogger<UpdateIntroVideoUrlTutorCommandHandler> logger)
    {
        _awsStorageService = awsStorageService;
        _cache = cache;
        _unitOfWork = unitOfWork;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(UpdateIntroVideoUrlTutorCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateIntroVideoUrlTutorCommand {@command}", command);
        try
        {
            var tutorExist = await _unitOfWork.GetRepository<Tutor, Guid>()
                .FindByIdAsync(command.Id, cancellationToken: cancellationToken);
            if (tutorExist == null)
            {
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Tutor>("Id"));
            }

            var updateFileResponse = await _awsStorageService.UpdateFileAsync(
                command.File, command.Id, nameof(PrefixFile.IntroVideo), cancellationToken);

            if (updateFileResponse == null)
            {
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.ValidationFailed("File"));
            }

            tutorExist.IntroVideoUrl = updateFileResponse.RelativePath;
            await _unitOfWork.SaveChangesAsync();

            var cacheKey = RedisKeyGenerator.GenerateTutorIntroVideoKey(tutorExist.Id);
            _ = _cache.SetCacheAsync(cacheKey, tutorExist.IntroVideoUrl,
                TimeSpan.FromMinutes(_settings.SettingCacheExpirationMinutes), false);

            _logger.LogInformation("End UpdateIntroVideoUrlTutorCommand {@command}", command);
            return Result.Success(updateFileResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateIntroVideoUrlTutorCommandHandler {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
