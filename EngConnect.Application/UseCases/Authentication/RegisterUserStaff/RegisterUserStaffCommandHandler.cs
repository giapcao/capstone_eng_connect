using System.Net;
using EngConnect.Application.UseCases.Authentication.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Authentication.RegisterUserStaff;

public class RegisterUserStaffCommandHandler : ICommandHandler<RegisterUserStaffCommand, RegisterUserStaffResponse>
{
    private readonly ILogger<RegisterUserStaffCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserStaffCommandHandler(
        ILogger<RegisterUserStaffCommandHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RegisterUserStaffResponse>> HandleAsync(
        RegisterUserStaffCommand command,
        CancellationToken cancellationToken = default)
    {
        Guid? transactionId = null;
        _logger.LogInformation("Start RegisterUserStaffCommandHandler {@Command}", command);
        
        try
        {
            // Check if user already exists
            var userRepo = _unitOfWork.GetRepository<User, Guid>();
            var isEmailExist = await userRepo.AnyAsync(
                x => x.Email == command.Email,
                cancellationToken);
            
            if (isEmailExist)
            {
                _logger.LogWarning("User already exists with email: {Email}", command.Email);
                return Result.Failure<RegisterUserStaffResponse>(
                    HttpStatusCode.BadRequest, UserErrors.UserAlreadyExists());
            }

            // Check if username already exists
            var isUsernameExist = await userRepo.AnyAsync(
                x => x.UserName == command.UserName,
                cancellationToken);
            
            if (isUsernameExist)
            {
                _logger.LogWarning("User already exists with username: {UserName}", command.UserName);
                return Result.Failure<RegisterUserStaffResponse>(
                    HttpStatusCode.BadRequest,  UserErrors.UserAlreadyExists());
            }

            var transaction = await _unitOfWork.BeginTransactionAsync();
            transactionId = transaction.TransactionId;

            // Hash password
            var hashedPassword = HashHelper.HashPassword(command.Password);
            
            // Create user with email verified (no email authentication needed)
            var user = User.Create(
                command.FirstName,
                command.LastName,
                command.UserName,
                command.Email,
                command.Phone,
                command.AddressNum,
                command.ProvinceId,
                command.ProvinceName,
                command.WardId,
                command.WardName,
                hashedPassword,
                nameof(UserStatus.Active));
            
            // Set email as verified since we don't send authentication email
            user.IsEmailVerified = true;
            
            userRepo.Add(user);

            // Assign Staff role
            var roleStaffCode = nameof(UserRoleEnum.Staff);
            var roleRepo = _unitOfWork.GetRepository<Role, Guid>();
            var staffRole = await roleRepo.FindFirstAsync(
                x => x.Code == roleStaffCode,
                cancellationToken: cancellationToken);
            
            if (staffRole != null)
            {
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = staffRole.Id
                };
                _unitOfWork.GetRepository<UserRole, Guid>().Add(userRole);
            }
            else
            {
                _logger.LogWarning("Role with code '{Code}' not found — skipping role assignment", roleStaffCode);
            }

            await _unitOfWork.SaveChangesAsync();

            // Commit transaction if all operations succeeded
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Committing transaction with {TransactionId}", transactionId);
                await _unitOfWork.CommitTransactionAsync();
            }

            // Create response
            var response = new RegisterUserStaffResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Status = user.Status
            };

            _logger.LogInformation("End RegisterUserStaffCommandHandler - Created staff user with ID: {UserId}", user.Id);
            return Result.Success<RegisterUserStaffResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in RegisterUserStaffCommandHandler: {Message}", ex.Message);
            
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Rolling back transaction with {TransactionId}", transactionId);
                await _unitOfWork.RollbackTransactionAsync();
            }
            
            return Result.Failure<RegisterUserStaffResponse>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
