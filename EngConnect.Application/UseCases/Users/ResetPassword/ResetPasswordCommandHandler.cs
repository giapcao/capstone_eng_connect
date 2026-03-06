using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Users.ResetPassword;

public class ResetPasswordCommandHandler: ICommandHandler<ResetPasswordCommand>
{
    //Check token and change new password for user
    private readonly ILogger<ResetPasswordCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _redisService;

    public ResetPasswordCommandHandler(ILogger<ResetPasswordCommandHandler> logger, IUnitOfWork unitOfWork, IRedisService redisService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _redisService = redisService;
    }

    public async Task<Result> HandleAsync(ResetPasswordCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start ResetPasswordCommandHandler {@Command}", command);
        try
        {
            var redisKey = RedisKeyGenerator.GenerateEmailResetPasswordTokenAsKey(command.Token);
            var storedUserIdStr = await _redisService.GetCacheAsync(redisKey);
            if (ValidationUtil.IsNullOrEmpty(storedUserIdStr))
            {
                _logger.LogWarning("Invalid or expired password reset token: {Token}", command.Token);
                return Result.Failure(HttpStatusCode.BadRequest, UserErrors.InvalidOrExpiredPasswordResetToken());
            }
            
            //Clean up the stored user Id string by removing quotes
            storedUserIdStr = storedUserIdStr.Trim('"');
            
            //Parse the stored user Id
            if (!Guid.TryParse(storedUserIdStr, out var userId))
            {
                _logger.LogWarning("Invalid user ID format in token: {Token}", command.Token);
                return Result.Failure(HttpStatusCode.BadRequest, UserErrors.InvalidOrExpiredPasswordResetToken());
            }  
            
            //Get user from database
            var userRepository = _unitOfWork.GetRepository<Domain.Persistence.Models.User, Guid>();
            var user = await userRepository.FindFirstAsync(x => x.Id == userId, cancellationToken: cancellationToken);
            if (ValidationUtil.IsNullOrEmpty(user))
            {
                _logger.LogWarning("User not found for ID: {UserId}", userId);
                return Result.Failure(HttpStatusCode.NotFound, UserErrors.UserNotFound());
            }
            
            //Hash new password
            var hashedPassword = HashHelper.HashPassword(command.NewPassword);
            //Update user password
            user.PasswordHash = hashedPassword;
            user.UpdatedAt = DateTime.UtcNow;
            
            userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            
            //Remove token from redis
            await _redisService.DeleteCacheAsync(redisKey);
            
            //Remove all refresh tokens for user
            var refreshTokenKeyPattern = RedisKeyGenerator.GenerateRefreshTokenKeyDeletePattern(user.Id);
            await _redisService.DeleteCacheWithPatternAsync(refreshTokenKeyPattern);
            
            _logger.LogInformation("End ResetPasswordCommandHandler");
            return Result.Success();
        }catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in ResetPasswordCommandHandler {@Command}", command);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }

    }
}