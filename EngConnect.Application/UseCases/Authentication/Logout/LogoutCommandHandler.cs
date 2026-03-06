using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.BuildingBlock.Presentation.Utils;
using EngConnect.Domain.Abstraction;
using EngConnect.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Authentication.Logout;

public class LogoutCommandHandler: ICommandHandler<LogoutCommand>
{
    private readonly ILogger<LogoutCommandHandler> _logger;
    private readonly IRedisService _redisService;
    private readonly IJwtTokenService _jwtTokenService;

    public LogoutCommandHandler(ILogger<LogoutCommandHandler> logger, IRedisService redisService, IJwtTokenService jwtTokenService)
    {
        _logger = logger;
        _redisService = redisService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result> HandleAsync(LogoutCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start LogoutCommandHandler {@Command}", command);
        try
        {
            //Validate the access token (without checking lifetime)
            var principal = _jwtTokenService.ValidateAccessToken(command.AccessToken, validateLifetime: false);
            if (principal == null)
            {
                _logger.LogWarning("Invalid token");
                return Result.Failure<bool>(HttpStatusCode.BadRequest, AuthErrors.Logout.InvalidToken());
            }

            // Extract userId from token
            var userId = principal.GetUserId();
            if (!userId.HasValue)
            {
                return Result.Failure<bool>(HttpStatusCode.BadRequest, AuthErrors.Logout.InvalidToken());
            }

            //Remove all refresh tokens of the user
            var pattern = RedisKeyGenerator.GenerateRefreshTokenKeyDeletePattern(userId.Value);
            await _redisService.DeleteCacheWithPatternAsync(pattern);

            _logger.LogInformation("End LogoutCommandHandler");
            return Result.Success();
        }catch (Exception e)
        {
            _logger.LogError(e, "Error in LogoutCommandHandler: {Message}", e.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}