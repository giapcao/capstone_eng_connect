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

namespace EngConnect.Application.UseCases.Tutors.GetIntroVideoUrlTutor;

public class GetIntroVideoUrlTutorQueryHandler : IQueryHandler<GetIntroVideoUrlTutorQuery, GetIntroVideoUrlTutorResponse>
{
    private readonly ILogger<GetIntroVideoUrlTutorQueryHandler> _logger;
    private readonly IAwsStorageService _awsStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _cache;
    private readonly RedisCacheSettings _settings;

    public GetIntroVideoUrlTutorQueryHandler(IUnitOfWork unitOfWork, IRedisService cache,
        IOptions<RedisCacheSettings> settings, IAwsStorageService awsStorageService,
        ILogger<GetIntroVideoUrlTutorQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _awsStorageService = awsStorageService;
        _settings = settings.Value;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Result<GetIntroVideoUrlTutorResponse>> HandleAsync(GetIntroVideoUrlTutorQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetIntroVideoUrlTutorQueryHandler {@Query}", query);
        try
        {
            var cacheKey = RedisKeyGenerator.GenerateTutorIntroVideoKey(query.Id);

            var value = await _cache.GetCacheAsync(cacheKey);
            if (value != null)
            {
                var fileCache = new GetIntroVideoUrlTutorResponse
                {
                    RelativePath = value,
                    Url = value != null ? _awsStorageService.GetFileUrl(value) : null
                };

                _logger.LogInformation("End GetIntroVideoUrlTutorQueryHandler (cache hit)");
                return Result.Success(fileCache);
            }

            var tutorExist = await _unitOfWork.GetRepository<Tutor, Guid>()
                .FindByIdAsync(query.Id, false, cancellationToken: cancellationToken);

            if (tutorExist == null)
            {
                return Result.Failure<GetIntroVideoUrlTutorResponse>(HttpStatusCode.NotFound,
                    CommonErrors.NotFound<Tutor>("Id"));
            }

            if (ValidationUtil.IsNullOrEmpty(tutorExist.IntroVideoUrl))
            {
                return Result.Failure<GetIntroVideoUrlTutorResponse>(HttpStatusCode.NotFound,
                    new Error("IntroVideoUrlNotFound", "Gia sư chưa upload video giới thiệu"));
            }

            var fileUrl = tutorExist.IntroVideoUrl != null ? _awsStorageService.GetFileUrl(tutorExist.IntroVideoUrl) : null;

            var file = new GetIntroVideoUrlTutorResponse
            {
                RelativePath = tutorExist.IntroVideoUrl,
                Url = fileUrl
            };

            _ = _cache.SetCacheAsync(cacheKey, tutorExist.IntroVideoUrl,
                TimeSpan.FromMinutes(_settings.SettingCacheExpirationMinutes), false);

            _logger.LogInformation("End GetIntroVideoUrlTutorQueryHandler");
            return Result.Success(file);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetIntroVideoUrlTutorQueryHandler {@Message}", ex.Message);
            return Result.Failure<GetIntroVideoUrlTutorResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
