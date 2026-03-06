using System.Net;
using EngConnect.Application.UseCases.Authentication.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Abstraction;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EngConnect.Application.UseCases.Authentication.LoginByUser;

public class LoginByUserCommandHandler: ICommandHandler<LoginByUserCommand, UserLoginResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LoginByUserCommandHandler> _logger;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRedisService _redisService;
    private readonly RedisCacheSettings _redisCacheSettings;
    public LoginByUserCommandHandler(IUnitOfWork unitOfWork, ILogger<LoginByUserCommandHandler> logger, IJwtTokenService jwtTokenService, 
        IRedisService redisService, IOptions<RedisCacheSettings> redisCacheSettings)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _jwtTokenService = jwtTokenService;
        _redisService = redisService;
        _redisCacheSettings = redisCacheSettings.Value;
    }

    public async Task<Result<UserLoginResponse>> HandleAsync(LoginByUserCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start LoginByCustomerCommandHandler {@Command}", command);
        try
        {
            var customerRepo =  _unitOfWork.GetRepository<User, Guid>();
            
            //Clean input
            var input = command.Email?.Trim().ToLowerInvariant();
            //Check customer exist
            var user = await customerRepo.FindFirstAsync(
                x => 
                    x.Email.ToLower() == input, 
                cancellationToken: cancellationToken);
            if (ValidationUtil.IsNullOrEmpty(user))
            {
                _logger.LogWarning("Customer not found with email: {email}", command.Email);
                return Result.Failure<UserLoginResponse>(
                    HttpStatusCode.NotFound,
                    UserErrors.UserNotFound());
            }
            
            //Verify password
            var isPasswordValid = HashHelper.VerifyPassword(command.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                _logger.LogWarning("Invalid password");
                return Result.Failure<UserLoginResponse>(HttpStatusCode.BadRequest, UserErrors.InvalidPassword());
            }
            
            //Generate token
            var accessToken = _jwtTokenService.GenerateAccessToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();
            
            //Store the new refresh token
            var newRefreshTokenKey = RedisKeyGenerator.GenerateRefreshTokenKey(user.Id, refreshToken);
            await _redisService.SetCacheAsync(newRefreshTokenKey, refreshToken,
                TimeSpan.FromMinutes(_redisCacheSettings.RefreshTokenExpirationMinutes));
            
            _logger.LogInformation("End LoginByCustomerCommandHandler");
            return Result.Success(new UserLoginResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error LoginByCustomerCommandHandler {@Command}", command);
            return Result.Failure<UserLoginResponse>(
                HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}