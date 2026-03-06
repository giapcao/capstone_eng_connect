using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Users.UpdateUser;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly ILogger<UpdateUserCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(ILogger<UpdateUserCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateUserCommandHandler {@Command}", command);
        try
        {
            var userRepository = _unitOfWork.GetRepository<User, Guid>();
            var user =
                await userRepository.FindByIdAsync(command.UserId, cancellationToken: cancellationToken);

            if (user is null)
            {
                _logger.LogWarning("User not found with Id: {UserId}", command.UserId);
                return Result.Failure(HttpStatusCode.NotFound, UserErrors.UserNotFound());
            }

            user.FirstName = command.FirstName ?? user.FirstName;
            user.LastName = command.LastName ?? user.LastName;


            // Update Phone
            if (ValidationUtil.IsNotNullOrEmpty(command.Phone) && command.Phone != user.Phone)
            {
                // Check if phone number is already in use by another user
                var isPhoneExist = await userRepository
                    .AnyAsync(u => u.Phone == command.Phone && u.Id != user.Id,
                        cancellationToken: cancellationToken);
                if (isPhoneExist)
                {
                    _logger.LogWarning("Phone number {Phone} is already in use by another user",
                        command.Phone);
                    return Result.Failure(HttpStatusCode.BadRequest, 
                        UserErrors.PhoneAlreadyExist());
                }

                user.Phone = command.Phone;
            }

            user.AddressNum = command.AddressNum ?? user.AddressNum;
            user.ProvinceId = command.ProvinceId ?? user.ProvinceId;
            user.ProvinceName = command.ProvinceName ?? user.ProvinceName;
            user.WardId = command.WardId ?? user.WardId;
            user.WardName = command.WardName ?? user.WardName;

            userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdateUserCommandHandler");
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occur in UpdateUserCommandHandler {@Message}", e.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}

