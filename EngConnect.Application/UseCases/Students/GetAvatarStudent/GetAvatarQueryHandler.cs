using System.Net;
using EngConnect.Application.UseCases.Students.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EngConnect.Application.UseCases.Students.GetAvatarStudent;

public class GetAvatarQueryHandler : IQueryHandler<GetAvatarQuery,GetAvatarResponse>
{
    private readonly ILogger<GetAvatarQueryHandler> _logger;
    private readonly IAwsStorageService _awsStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _cache;
    private readonly RedisCacheSettings _settings;

    public GetAvatarQueryHandler(IUnitOfWork unitOfWork, IRedisService cache, IOptions<RedisCacheSettings> settings, IAwsStorageService awsStorageService, ILogger<GetAvatarQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _awsStorageService =  awsStorageService;
        _settings = settings.Value;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Result<GetAvatarResponse>> HandleAsync(GetAvatarQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetAvatarQueryHandler {@Query}", query);
        try
        {
            var cacheKey = query.Id.ToString();
            
            var value = await _cache.GetCacheAsync(cacheKey);
            if (value != null)
            {
                var fileCache = new GetAvatarResponse
                {
                    RelativePath = value,
                    Url = _awsStorageService.GetFileUrl(value, cancellationToken)
                };
                
                _logger.LogInformation("End GetAvatarQueryHandler");
                return Result.Success(fileCache);
            }
            
            var studentExist = await _unitOfWork.GetRepository<Student, Guid>()
                .FindByIdAsync(query.Id, false, cancellationToken: cancellationToken);
            
            if (studentExist == null)
            {
                return Result.Failure<GetAvatarResponse>(HttpStatusCode.NotFound, CommonErrors.NotFound<Student>("Id"));
            }

            if (ValidationUtil.IsNullOrEmpty(studentExist.Avatar))
            {
                return Result.Failure<GetAvatarResponse>(HttpStatusCode.NotFound, CommonErrors.NotFound<Student>("Avatar"));
            }
            
            var fileUrl = _awsStorageService.GetFileUrl(studentExist.Avatar,cancellationToken);
            
            var file = new GetAvatarResponse
            {
                RelativePath = studentExist.Avatar,
                Url = fileUrl
            };
            
            _= _cache.SetCacheAsync(cacheKey,studentExist.Avatar,TimeSpan.FromDays(_settings.SettingCacheExpirationDays));
            return Result.Success(file);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateAvatarStudentCommandHandler {@Message}", ex.Message);
            return Result.Failure<GetAvatarResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}