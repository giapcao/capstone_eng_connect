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

namespace EngConnect.Application.UseCases.Tutors.UpdateAvatarTutor;

public class UpdateAvatarTutorCommandHandler : ICommandHandler<UpdateAvatarTutorCommand>
{
    private readonly ILogger<UpdateAvatarTutorCommandHandler> _logger;
    private readonly IAwsStorageService _awsStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _cache;
    private readonly RedisCacheSettings _settings;

    public UpdateAvatarTutorCommandHandler(IAwsStorageService awsStorageService,
        IOptions<RedisCacheSettings> settings, IRedisService cache,
        IUnitOfWork unitOfWork, ILogger<UpdateAvatarTutorCommandHandler> logger)
    {
        _awsStorageService = awsStorageService;
        _cache = cache;
        _unitOfWork = unitOfWork;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(UpdateAvatarTutorCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateAvatarTutorCommand {@command}", command);
        try
        {
            var tutorExist = await _unitOfWork.GetRepository<Tutor, Guid>()
                .FindByIdAsync(command.Id, cancellationToken: cancellationToken);
            if (tutorExist == null)
            {
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Tutor>("Id"));
            }

            var updateFileRequest = await _awsStorageService.UpdateFileAsync(
                command.File, command.Id, nameof(PrefixFile.Avatar), cancellationToken);

            if (updateFileRequest == null)
            {
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.ValidationFailed("File"));
            }

            tutorExist.Avatar = updateFileRequest.RelativePath;
            await _unitOfWork.SaveChangesAsync();

            var cacheKey = RedisKeyGenerator.GenerateTutorAvatarKey(tutorExist.Id);
            _ = _cache.SetCacheAsync(cacheKey, tutorExist.Avatar,
                TimeSpan.FromMinutes(_settings.SettingCacheExpirationMinutes), false);

            _logger.LogInformation("End UpdateAvatarTutorCommand {@command}", command);
            return Result.Success(updateFileRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateAvatarTutorCommandHandler {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
