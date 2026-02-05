using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.BuildingBlock.EventBus.Constants;
using EngConnect.BuildingBlock.EventBus.Events;
using EngConnect.BuildingBlock.EventBus.Utils;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EngConnect.Application.UseCases.Users.ForgotPassword;

public class ForgotPasswordCommandHandler: ICommandHandler<ForgotPasswordCommand>
{
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _redisService;
    private readonly RedisCacheSettings _redisCacheSettings;

    public ForgotPasswordCommandHandler(ILogger<ForgotPasswordCommandHandler> logger, IUnitOfWork unitOfWork, 
        IRedisService redisService, IOptions<RedisCacheSettings> redisCacheSettings)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _redisService = redisService;
        _redisCacheSettings = redisCacheSettings.Value;
    }

    public async Task<Result> HandleAsync(ForgotPasswordCommand command, CancellationToken cancellationToken = default)
    {
        //Send reset password token to user email
        _logger.LogInformation("Start ResetPasswordCommandHandler {@Command}", command);
        try
        {
            //Check user exist
            var user = await _unitOfWork.GetRepository<User, Guid>()
                .FindSingleAsync(x => x.Email == command.Email, 
                    cancellationToken: cancellationToken);
            if (ValidationUtil.IsNullOrEmpty(user))
            {
                _logger.LogWarning("User not found with email: {Email}", command.Email);
                return Result.Failure(
                    HttpStatusCode.NotFound,
                    UserErrors.UserNotFound());
            }
            
            //Generate reset password token
            var resetToken = GenerateResetToken();
            var tokenExpiration = TimeSpan.FromMinutes(_redisCacheSettings.EmailResetPasswordTokenExpirationInMinutes);
            
            
            //Persist reset password event in database
            var resetPasswordEvent = ResetPasswordEvent.Create(user.Id, user.Email,
                user.FirstName + " " + user.LastName, resetToken);
            
            
            //Create notification event
            var notificationEvent = NotificationHelper.CreateNotification(resetPasswordEvent, [user.Id], [],
                nameof(Channel.Email));
            
            //Create outbox event
            var outboxEvent = OutboxEvent.CreateOutboxEvent(nameof(User), user.Id, notificationEvent);
            
            _unitOfWork.GetRepository<OutboxEvent, Guid>().Add(outboxEvent);
            await _unitOfWork.SaveChangesAsync();
            
            //Store token in redis
            var tokenKey = RedisKeyGenerator.GenerateEmailResetPasswordTokenAsKey(resetToken);
            await _redisService.SetCacheAsync(tokenKey, user.Id.ToString(), tokenExpiration);
            
            _logger.LogInformation("End ResetPasswordCommandHandler");
            return Result.Success();
        }catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in ResetPasswordCommandHandler {@Command}", command);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
    
    // Generate a simple reset token
    private string GenerateResetToken()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}