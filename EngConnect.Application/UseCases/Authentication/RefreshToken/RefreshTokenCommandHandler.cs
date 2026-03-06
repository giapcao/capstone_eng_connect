using System.Net;
using EngConnect.Application.UseCases.Authentication.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Presentation.Utils;
using EngConnect.Domain.Abstraction;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EngConnect.Application.UseCases.Authentication.RefreshToken;

public class RefreshTokenCommandHandler: ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly ILogger<RefreshTokenCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _redisService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly RedisCacheSettings _redisCacheSettings;

    public RefreshTokenCommandHandler(ILogger<RefreshTokenCommandHandler> logger, IUnitOfWork unitOfWork, 
        IRedisService redisService, IJwtTokenService jwtTokenService, IOptions<RedisCacheSettings> redisCacheSettings)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _redisService = redisService;
        _jwtTokenService = jwtTokenService;
        _redisCacheSettings = redisCacheSettings.Value;
    }

    public async Task<Result<RefreshTokenResponse>> HandleAsync(RefreshTokenCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("Start RefreshTokenCommandHandler {@Command}", command);
        try
        {
            //Validate the expired access token (without checking lifetime)
            var principle = _jwtTokenService.ValidateAccessToken(command.AccessToken, validateLifetime: false);
            if (ValidationUtil.IsNullOrEmpty(principle))
            {
                _logger.LogWarning("Invalid access token");
                return Result.Failure<RefreshTokenResponse>(HttpStatusCode.BadRequest, AuthErrors.RefreshToken.InvalidToken());
            }
            
            //Extract user id from access token
            var userId = principle.GetUserId();
            if (ValidationUtil.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("UserId not found in access token");
                return Result.Failure<RefreshTokenResponse>(HttpStatusCode.BadRequest, AuthErrors.RefreshToken.InvalidToken());
            }
            
            //Check if refresh token exists in Redis
            var refreshTokenKey = RedisKeyGenerator.GenerateRefreshTokenKey(userId.Value, command.RefreshToken);
            var storedToken = await _redisService.GetCacheAsync(refreshTokenKey);
            if (ValidationUtil.IsNullOrEmpty(storedToken))
            {
                _logger.LogWarning("Refresh token not found or expired in Redis for userId: {UserId}", userId);
                return Result.Failure<RefreshTokenResponse>(HttpStatusCode.BadRequest, AuthErrors.RefreshToken.InvalidToken());
            }
            
            var userRole = principle.GetRoles();
            string? accessToken;
            //Verify the user with still exist and is active
            var user = await _unitOfWork.GetRepository<User, Guid>().FindSingleAsync(
                c => c.Id == userId.Value && c.Status == nameof(UserStatus.Active),
                cancellationToken: cancellationToken);

            if (ValidationUtil.IsNullOrEmpty(user))
            {
                _logger.LogWarning("User not found or inactive for userId: {UserId}",
                    userId);
                return Result.Failure<RefreshTokenResponse>(HttpStatusCode.BadRequest, UserErrors.UserNotFound());
            }
            
            accessToken = _jwtTokenService.GenerateAccessToken(user);
            
            //Generate new refresh token
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            //Delete old refresh token from Redis
            await _redisService.DeleteCacheAsync(refreshTokenKey);
            
            //Store new refresh token in Redis
            var newRefreshTokenKey = RedisKeyGenerator.GenerateRefreshTokenKey(user.Id, refreshToken);
            await _redisService.SetCacheAsync(newRefreshTokenKey, refreshToken,
                TimeSpan.FromMinutes(_redisCacheSettings.RefreshTokenExpirationMinutes));
            
            _logger.LogInformation("End RefreshTokenCommandHandler");
            
            return new RefreshTokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }catch (Exception ex)
        {
            _logger.LogError(ex, "Error in RefreshTokenCommandHandler {@Command}", command);
            return Result.Failure<RefreshTokenResponse>(HttpStatusCode.InternalServerError, AuthErrors.ErrorInWhileRefreshToken());
        }
    }
}