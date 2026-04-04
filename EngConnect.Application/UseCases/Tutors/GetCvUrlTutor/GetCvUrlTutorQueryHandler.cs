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

namespace EngConnect.Application.UseCases.Tutors.GetCvUrlTutor;

public class GetCvUrlTutorQueryHandler : IQueryHandler<GetCvUrlTutorQuery, GetCvUrlTutorResponse>
{
    private readonly ILogger<GetCvUrlTutorQueryHandler> _logger;
    private readonly IAwsStorageService _awsStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _cache;
    private readonly RedisCacheSettings _settings;

    public GetCvUrlTutorQueryHandler(IUnitOfWork unitOfWork, IRedisService cache,
        IOptions<RedisCacheSettings> settings, IAwsStorageService awsStorageService,
        ILogger<GetCvUrlTutorQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _awsStorageService = awsStorageService;
        _settings = settings.Value;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Result<GetCvUrlTutorResponse>> HandleAsync(GetCvUrlTutorQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetCvUrlTutorQueryHandler {@Query}", query);
        try
        {
            var cacheKey = RedisKeyGenerator.GenerateTutorCvKey(query.Id);

            var value = await _cache.GetCacheAsync(cacheKey);
            if (value != null)
            {
                var fileCache = new GetCvUrlTutorResponse
                {
                    RelativePath = value,
                    Url = value != null ? _awsStorageService.GetFileUrl(value) : null
                };

                _logger.LogInformation("End GetCvUrlTutorQueryHandler (cache hit)");
                return Result.Success(fileCache);
            }

            var tutorExist = await _unitOfWork.GetRepository<Tutor, Guid>()
                .FindByIdAsync(query.Id, false, cancellationToken: cancellationToken);

            if (tutorExist == null)
            {
                return Result.Failure<GetCvUrlTutorResponse>(HttpStatusCode.NotFound,
                    CommonErrors.NotFound<Tutor>("Id"));
            }

            if (ValidationUtil.IsNullOrEmpty(tutorExist.CvUrl))
            {
                return Result.Failure<GetCvUrlTutorResponse>(HttpStatusCode.NotFound,
                    new Error("CvUrlNotFound", "Gia sư chưa upload CV"));
            }

            var fileUrl = tutorExist.CvUrl != null ? _awsStorageService.GetFileUrl(tutorExist.CvUrl) : null;

            var file = new GetCvUrlTutorResponse
            {
                RelativePath = tutorExist.CvUrl,
                Url = fileUrl
            };

            _ = _cache.SetCacheAsync(cacheKey, tutorExist.CvUrl,
                TimeSpan.FromMinutes(_settings.SettingCacheExpirationMinutes), false);

            _logger.LogInformation("End GetCvUrlTutorQueryHandler");
            return Result.Success(file);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetCvUrlTutorQueryHandler {@Message}", ex.Message);
            return Result.Failure<GetCvUrlTutorResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
