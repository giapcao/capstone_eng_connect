using System.Net;
using EngConnect.Application.UseCases.Authentication.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin;

/// <summary>
/// Get data for user login with Google method
/// </summary>
public class VerifyGoogleLoginCommandHandler: ICommandHandler<VerifyGoogleLoginCommand, UserLoginResponse>
{
    private readonly IRedisService _redisService;
    private ILogger<VerifyGoogleLoginCommandHandler> _logger;

    public VerifyGoogleLoginCommandHandler(IRedisService redisService, ILogger<VerifyGoogleLoginCommandHandler> logger)
    {
        _redisService = redisService;
        _logger = logger;
    }

    public async Task<Result<UserLoginResponse>> HandleAsync(VerifyGoogleLoginCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start VerifyGoogleLoginCommandHandler {@Command}", command);
        try
        {
            if (ValidationUtil.IsNullOrEmpty(command.Token))
            {
                _logger.LogWarning("VerifyGoogleLoginCommandHandler failed: Token is null or empty");
                return Result.Failure<UserLoginResponse>(HttpStatusCode.BadRequest, AuthErrors.GoogleOAuth.InvalidToken());
            }
            
            //Get data from redis
            var cacheKey = RedisKeyGenerator.GenerateUserLoginTokenKey(command.Token);
            var loginResponse =  await _redisService.GetCacheAsync<UserLoginResponse>(cacheKey);
            
            //Check if data exist in redis
            if (ValidationUtil.IsNullOrEmpty(loginResponse))
            {
                _logger.LogWarning("No login data found in Redis for token: {Token}", command.Token);
                return Result.Failure<UserLoginResponse>(HttpStatusCode.BadRequest,
                    AuthErrors.GoogleOAuth.InvalidToken());
            }
            
            //Delete the cache entry (one-time use)
            await _redisService.DeleteCacheAsync(cacheKey);
            
            _logger.LogInformation("End VerifyGoogleLoginCommandHandler");
            return Result.Success(loginResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured in VerifyGoogleLoginCommandHandler: {Message}", ex.Message);
            return Result.Failure<UserLoginResponse>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}