using System.Net;
using EngConnect.Application.UseCases.Tutors.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EngConnect.Application.UseCases.Tutors.GetAvatarTutor;

public class GetAvatarTutorQueryHandler : IQueryHandler<GetAvatarTutorQuery, GetAvatarTutorResponse>
{
    private readonly ILogger<GetAvatarTutorQueryHandler> _logger;
    private readonly IAwsStorageService _awsStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _cache;
    private readonly RedisCacheSettings _settings;

    public GetAvatarTutorQueryHandler(IUnitOfWork unitOfWork, IRedisService cache,
        IOptions<RedisCacheSettings> settings, IAwsStorageService awsStorageService,
        ILogger<GetAvatarTutorQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _awsStorageService = awsStorageService;
        _settings = settings.Value;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Result<GetAvatarTutorResponse>> HandleAsync(GetAvatarTutorQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetAvatarTutorQueryHandler {@Query}", query);
        try
        {
            var cacheKey = RedisKeyGenerator.GenerateTutorAvatarKey(query.Id);

            var value = await _cache.GetCacheAsync(cacheKey);
            if (value != null)
            {
                var fileCache = new GetAvatarTutorResponse
                {
                    RelativePath = value,
                    Url = value != null ? _awsStorageService.GetFileUrl(value) : null
                };

                _logger.LogInformation("End GetAvatarTutorQueryHandler (cache hit)");
                return Result.Success(fileCache);
            }

            var tutorExist = await _unitOfWork.GetRepository<Tutor, Guid>()
                .FindByIdAsync(query.Id, false, cancellationToken: cancellationToken);

            if (tutorExist == null)
            {
                return Result.Failure<GetAvatarTutorResponse>(HttpStatusCode.NotFound,
                    CommonErrors.NotFound<Tutor>("Id"));
            }

            if (ValidationUtil.IsNullOrEmpty(tutorExist.Avatar))
            {
                return Result.Failure<GetAvatarTutorResponse>(HttpStatusCode.NotFound,
                    CommonErrors.NotFound<Tutor>("Avatar"));
            }

            var fileUrl = tutorExist.Avatar != null ? _awsStorageService.GetFileUrl(tutorExist.Avatar) : null;

            var file = new GetAvatarTutorResponse
            {
                RelativePath = tutorExist.Avatar,
                Url = fileUrl
            };

            _ = _cache.SetCacheAsync(cacheKey, tutorExist.Avatar,
                TimeSpan.FromMinutes(_settings.SettingCacheExpirationMinutes), false);

            _logger.LogInformation("End GetAvatarTutorQueryHandler");
            return Result.Success(file);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetAvatarTutorQueryHandler {@Message}", ex.Message);
            return Result.Failure<GetAvatarTutorResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
