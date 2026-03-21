using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.BuildingBlock.EventBus.Constants;
using EngConnect.BuildingBlock.EventBus.Events;
using EngConnect.BuildingBlock.EventBus.Utils;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EngConnect.Application.UseCases.Authentication.RegisterUser;

public class RegisterUserCommandHandler: ICommandHandler<RegisterUserCommand>
{
    private readonly ILogger<RegisterUserCommandHandler> _logger;
    private readonly IRedisService _redisService;
    private readonly RedisCacheSettings _redisCacheSettings;
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppSettings _appSettings;


    public RegisterUserCommandHandler(ILogger<RegisterUserCommandHandler> logger, IRedisService redisService, 
        IOptions<RedisCacheSettings> redisCacheSettings, IUnitOfWork unitOfWork, 
        IOptions<AppSettings> appSettings)
    {
        _logger = logger;
        _redisService = redisService;
        _redisCacheSettings = redisCacheSettings.Value;
        _unitOfWork = unitOfWork;
        _appSettings = appSettings.Value;
    }

    public async Task<Result> HandleAsync(RegisterUserCommand command, CancellationToken cancellationToken = default)
    {
        //Track if we are using a transaction
        Guid? transactionId = null;
        _logger.LogInformation("Start RegisterUserCommandHandler {@Command}", command);
        try
        {
            //Check user exist
            var userRepo = _unitOfWork.GetRepository<User,Guid>();
            var isEmailExist = await userRepo.AnyAsync(
                x => x.Email == command.Email,
                cancellationToken);
            if (isEmailExist)
            {
                _logger.LogWarning("User already exists with email: {Email}", command.Email);
                return Result.Failure(
                    HttpStatusCode.BadRequest,
                    UserErrors.UserAlreadyExists());
            }
            var transaction = await _unitOfWork.BeginTransactionAsync();
            transactionId = transaction.TransactionId;
            //Hash password
            var hashedPassword = HashHelper.HashPassword(command.Password);
            //Create user
            var user = User.Create(command.FirstName, command.LastName, 
                command.UserName, command.Email, command.Phone, command.AddressNum,
                command.ProvinceId, command.ProvinceName, command.WardId, command.WardName,
                hashedPassword, nameof(UserStatus.Active));
            
            userRepo.Add(user);

            //Create student profile
            var student = Student.CreateStudentWithUserId(
                user.Id, command.School, command.Grade, command.Class, _appSettings.DefaultAvatarPath);
            if (!string.IsNullOrWhiteSpace(command.Notes))
                student.Notes = command.Notes;
            _unitOfWork.GetRepository<Student, Guid>().Add(student);

            //Assign Student role
            var roleStudentCode = nameof(UserRoleEnum.Student);
            var roleRepo = _unitOfWork.GetRepository<Role, Guid>();
            var studentRole = await roleRepo.FindFirstAsync(
                x => x.Code == roleStudentCode,
                cancellationToken: cancellationToken);
            if (studentRole != null)
            {
                var userRole = new UserRole { UserId = user.Id, RoleId = studentRole.Id };
                _unitOfWork.GetRepository<UserRole, Guid>().Add(userRole);
            }
            else
            {
                _logger.LogWarning("Role with code '{Code}' not found — skipping role assignment", roleStudentCode);
            }

            //Generate email verification code
            var verificationCode = Guid.NewGuid().ToString("N");
            
            //Persist user register event in outbox
            //Create event with exist template
            var @event = UserRegisterEvent.Create(user.Id, user.Email, 
                user.FirstName + " " + user.LastName,
                verificationCode);
            //Create notification event
            var notificationEvent = NotificationHelper.CreateNotification(@event,
                [user.Id], [], nameof(Channel.Email));
            
            //Create outbox event
            var outboxEvent = OutboxEvent.CreateOutboxEvent(nameof(User), user.Id,
                notificationEvent);
            //Add to outbox repository
            var outboxRepo = _unitOfWork.GetRepository<OutboxEvent, Guid>();
            outboxRepo.Add(outboxEvent);
            await _unitOfWork.SaveChangesAsync();
            
            //Generate verification token and store in Redis
            var redisKey = RedisKeyGenerator.GenerateEmailVerificationTokenAsKey(verificationCode);
            var tokenExpiration = TimeSpan.FromMinutes(_redisCacheSettings.EmailVerificationTokenExpirationMinutes);
            await _redisService.SetCacheAsync(redisKey, user.Id.ToString(), tokenExpiration);
            
            
            //Commit transaction if all operations succeeded
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Committing transaction with {TransactionId}", transactionId);
                await _unitOfWork.CommitTransactionAsync();
            }
            
            _logger.LogInformation("End RegisterUserCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error in RegisterUserCommandHandler: {Message}", ex.Message);
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Rolling back transaction with {TransactionId}", transactionId);
                await _unitOfWork.RollbackTransactionAsync();
            }
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());

        }
    }
}
