using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Users.ChangePassword;

public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ChangePasswordCommandHandler> _logger;

    public ChangePasswordCommandHandler(IUnitOfWork unitOfWork, ILogger<ChangePasswordCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(ChangePasswordCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start ChangePasswordCommandHandler: {@Command}", command);
        try
        {
            var userRepo = _unitOfWork.GetRepository<User, Guid>();

            var user = await userRepo.FindSingleAsync(u => u.Id == command.UserId, cancellationToken: cancellationToken);

            if (ValidationUtil.IsNullOrEmpty(user))
            {
                _logger.LogWarning("User not found with Id: {UserId}", command.UserId);
                return Result.Failure(HttpStatusCode.BadRequest, UserErrors.UserNotFound());
            }

            var isOldPasswordValid = HashHelper.VerifyPassword(command.OldPassword, user.PasswordHash);

            if (!isOldPasswordValid)
            {
                _logger.LogWarning("Old password is incorrect for user {UserId}", command.UserId);
                return Result.Failure(HttpStatusCode.BadRequest, 
                    UserErrors.InvalidPassword());
            }

            user.PasswordHash = HashHelper.HashPassword(command.NewPassword);

            userRepo.Update(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End ChangePasswordCommandHandler");
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ChangePasswordCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}

