using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Authentication.VerifyEmail;

public class VerifyEmailCommandHandler: ICommandHandler<VerifyEmailCommand>
{
    private readonly ILogger<VerifyEmailCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _redisService;

    public VerifyEmailCommandHandler(ILogger<VerifyEmailCommandHandler> logger, IUnitOfWork unitOfWork, IRedisService redisService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _redisService = redisService;
    }

    public async Task<Result> HandleAsync(VerifyEmailCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start VerifyEmailCommandHandler {@Command}", command);
        try
        {
            //Check if token exits in redis
            var tokenKey = RedisKeyGenerator.GenerateEmailVerificationTokenAsKey(command.Token);
            var storeUserIdStr = await _redisService.GetCacheAsync(tokenKey);
            
            if (ValidationUtil.IsNullOrEmpty(storeUserIdStr))
            {
                _logger.LogWarning("Invalid or expired email verification token: {Token}", command.Token);
                return Result.Failure(HttpStatusCode.BadRequest, AuthErrors.InvalidOrExpiredEmailVerificationToken());
            }
            
            //Clean up the store user Id string by removing quotes
            storeUserIdStr = storeUserIdStr.Trim('"');
            
            //Parse the stored user Id
            if (!Guid.TryParse(storeUserIdStr, out var storeUserId))
            {
                _logger.LogWarning("Invalid user ID format in token: {Token}", command.Token);
                return Result.Failure(HttpStatusCode.BadRequest, AuthErrors.InvalidOrExpiredEmailVerificationToken());
            }
            
            //Get user from database
            var userRepository = _unitOfWork.GetRepository<Domain.Persistence.Models.User, Guid>();
            var user = await userRepository.FindFirstAsync(x => x.Id == storeUserId, cancellationToken: cancellationToken);
            
            if (ValidationUtil.IsNullOrEmpty(user))
            {
                _logger.LogWarning("User not found for ID: {UserId}", storeUserId);
                return Result.Failure(HttpStatusCode.NotFound, UserErrors.UserNotFound());
            }
            
            //Update user verified status
            user.IsEmailVerified = true;
            userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            
            //Remove token from redis
            await _redisService.DeleteCacheAsync(tokenKey);
            
            _logger.LogInformation("End VerifyEmailCommandHandler");
            return Result.Success();
        }catch (Exception ex)
        {
            _logger.LogError(ex, "Error in VerifyEmailCommandHandler {@Command}", command);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}