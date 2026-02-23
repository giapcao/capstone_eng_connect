using System.Net;
using System.Security.Claims;
using EngConnect.Application.UseCases.Authentication.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.BuildingBlock.EventBus.Constants;
using EngConnect.BuildingBlock.EventBus.Events;
using EngConnect.BuildingBlock.EventBus.Utils;
using EngConnect.Domain.Abstraction;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EngConnect.Application.UseCases.Authentication.LoginWithGoogleOAuth;

public class LoginWithGoogleOAuthCommandHandler: ICommandHandler<LoginWithGoogleOAuthCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _redisService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<LoginWithGoogleOAuthCommandHandler> _logger;
    private readonly RedisCacheSettings _redisCacheSettings;
    private readonly RedirectUrlSettings _redisRedirectUrlSettings;
    private readonly IMessageBusWithOutboxService _messageBusWithOutboxService;

    public LoginWithGoogleOAuthCommandHandler(
        IUnitOfWork unitOfWork, 
        IRedisService redisService, 
        IJwtTokenService jwtTokenService, 
        ILogger<LoginWithGoogleOAuthCommandHandler> logger, 
        IOptions<RedisCacheSettings> redisCacheSettings, 
        IOptions<RedirectUrlSettings> redisRedirectUrlSettings, 
        IMessageBusWithOutboxService messageBusWithOutboxService)
    {
        _unitOfWork = unitOfWork;
        _redisService = redisService;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
        _redisCacheSettings = redisCacheSettings.Value;
        _redisRedirectUrlSettings = redisRedirectUrlSettings.Value;
        _messageBusWithOutboxService = messageBusWithOutboxService;
    }

    public async Task<Result<string>> HandleAsync(LoginWithGoogleOAuthCommand command, CancellationToken cancellationToken = default)
    {
        // Log user info
        var objForLog = new
        {
            AuthType = command.Principal?.Identity?.AuthenticationType,
            Name = command.Principal?.FindFirst(ClaimTypes.Name)?.Value,
            GivenName = command.Principal?.FindFirst(ClaimTypes.GivenName)?.Value,
            Surname = command.Principal?.FindFirst(ClaimTypes.Surname)?.Value,
            Email = command.Principal?.FindFirst(ClaimTypes.Email)?.Value,
        };
        
        //Track if we are using a transaction
        Guid? transactionId = null;
        
        _logger.LogInformation("Start LoginByGoogleOAuthCommandHandler {@ObjForLog}", objForLog);
        try
        {
            if (ValidationUtil.IsNullOrEmpty(command.Principal))
            {
                _logger.LogWarning("LoginByGoogleOAuthCommandHandler failed: Email claim is missing in the principal.");
                return Result.Failure<string>(HttpStatusCode.BadRequest, AuthErrors.GoogleOAuth.AuthenticationFailed());
            }
            
            //Extract email from claims
            var email = command.Principal.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            if (ValidationUtil.IsNullOrEmpty(email))
            {
                _logger.LogWarning("LoginByGoogleOAuthCommandHandler failed: Email claim is missing in the principal.");
                return Result.Failure<string>(HttpStatusCode.BadRequest, AuthErrors.GoogleOAuth.EmailNotFound());
            }
            
            //Check if user exists
            var userRepo = _unitOfWork.GetRepository<Domain.Persistence.Models.User, Guid>();
            var user = await userRepo.FindFirstAsync(x => x.Email == email, cancellationToken: cancellationToken);

            if (ValidationUtil.IsNullOrEmpty(user))
            {
                _logger.LogInformation("User with email {Email} not found, creating new user...", email);

                //Start transaction 
                var transaction = await _unitOfWork.BeginTransactionAsync();
                transactionId = transaction.TransactionId;
                _logger.LogDebug("Begin transaction with {TransactionId}", transactionId);

                //Create new user
                //Extract data from principal
                var fullName = command.Principal.FindFirst(x => x.Type == ClaimTypes.Name)?.Value;
                var firstName = string.Empty;
                var lastName = string.Empty;
                if (ValidationUtil.IsNullOrEmpty(fullName))
                {
                    fullName = email.Split("@")[0]; // Use email prefix as name if not available
                    firstName = fullName;
                }
                else
                {
                    var nameParts = fullName.Split(' ', 2);
                    // E.g. "Giap Cao Dinh" => firstName = "Giap", lastName = "Cao Dinh"
                    firstName = nameParts[0];
                    lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;
                }
                
                //Generate a password hashing using the random GUID (user don't need password with login with Google)
                var password = Guid.NewGuid().ToString("N");
                var passwordHash = HashHelper.HashPassword(password);
                
                //Create new user
                user = User.CreateUserGoogle(
                    firstName,
                    lastName,
                    email,
                    email,
                    passwordHash,
                    nameof(UserStatus.Active));
                
                userRepo.Add(user);
                
                //Persist user google register event in outbox for welcome email
                var @event = UserRegisterByGoogleOAuthEvent.Create(
                    user.Id, 
                    user.Email,
                    user.FirstName + " " +  user.LastName, 
                    password);
                
                var notificationEvent = NotificationHelper.CreateNotification(
                    @event, [user.Id], [], nameof(Channel.Email));
                
                // This ensures atomicity: if anything fails, both customer and event are rolled back
                var outboxEvent = OutboxEvent.CreateOutboxEvent(
                    nameof(User),
                    user.Id,
                    notificationEvent);
                
                _unitOfWork.GetRepository<OutboxEvent, Guid>().Add(outboxEvent);

                await _unitOfWork.SaveChangesAsync();
            }

            else
            {
                _logger.LogInformation("Existing user with email {Email} found, skipping user creation.", email);
            }
            
            //Generate JWT token (can throw exception - will trigger rollback if in transaction)
            var accessToken =  _jwtTokenService.GenerateAccessToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();
            
            //Store refresh token in Redis (can throw exception - will trigger rollback if in transaction)
            await _redisService.SetCacheAsync(
                RedisKeyGenerator.GenerateRefreshTokenKey(user.Id, refreshToken),
                refreshToken, TimeSpan.FromMinutes(_redisCacheSettings.RefreshTokenExpirationMinutes));
            
            //Generate login response
            var loginResponse = new UserLoginResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
            
            //Generate temporary cache key for redirect URL after login success
            var token =  Guid.NewGuid().ToString("N");
            
            //Store payload in Redis with short expiration (e.g. 5 minutes)
            await _redisService.SetCacheAsync(
                RedisKeyGenerator.GenerateUserLoginTokenKey(token),
                loginResponse, TimeSpan.FromMinutes(_redisCacheSettings.UserLoginTokenExpirationInMinutes));
            
            //Redirect to frontend with token 
            var encodedToken = Uri.EscapeDataString(token);
            var redirectUrl = _redisRedirectUrlSettings.GoogleLoginSuccessUrl + encodedToken;

            //Commit transaction if all operations succeeded
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Committing transaction with {TransactionId}", transactionId);
                await _unitOfWork.CommitTransactionAsync();
            }
            
            _logger.LogInformation("Redirect Url: {RedirectUrl}", redirectUrl);
            _logger.LogInformation("End LoginByGoogleOAuthCommandHandler");
            return Result.Success(redirectUrl);

        }
        catch (Exception ex)
        {
            _logger.LogError("Error occured in LoginByGoogleOAuthCommandHandler: {Message}", ex.Message);
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Rolling back transaction with {TransactionId}", transactionId);
                await _unitOfWork.RollbackTransactionAsync();
            }

            return Result.Failure<string>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}