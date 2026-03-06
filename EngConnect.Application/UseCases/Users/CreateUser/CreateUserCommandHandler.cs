using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Users.CreateUser;

/// <summary>
/// Dùng để tạo user - staff (Dùng bởi admin)
/// </summary>
public class CreateUserCommandHandler: ICommandHandler<CreateUserCommand>
{
    private readonly ILogger<CreateUserCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateUserCommandHandler {@Command}", command);
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
            
            //Hash password
            var hashedPassword = HashHelper.HashPassword(command.Password);
            //Create user
            var user = new User
            {
                UserName = command.UserName,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                PasswordHash = hashedPassword,
                Status = nameof(UserStatus.Active),
                IsEmailVerified = true
            };
            
            userRepo.Add(user);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("End CreateUserCommandHandler");
            return Result.Success();
        }catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateUserCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
        
    }
}
