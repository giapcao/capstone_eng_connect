using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.UserRoles.CreateUserRole;

public class CreateUserRoleCommandHandler : ICommandHandler<CreateUserRoleCommand>
{
    private readonly ILogger<CreateUserRoleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserRoleCommandHandler(ILogger<CreateUserRoleCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateUserRoleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateUserRoleCommandHandler {@Command}", command);
        try
        {
            var userRoleRepo = _unitOfWork.GetRepository<UserRole, Guid>();
            var userRepo = _unitOfWork.GetRepository<User, Guid>();
            var roleRepo = _unitOfWork.GetRepository<Role, Guid>();

            // Check if user exists
            var userExists = await userRepo.AnyAsync(x => x.Id == command.UserId, cancellationToken);
            if (!userExists)
            {
                _logger.LogWarning("User not found with ID: {UserId}", command.UserId);
                return Result.Failure(HttpStatusCode.NotFound, UserErrors.UserNotFound());
            }

            // Check if role exists
            var roleExists = await roleRepo.AnyAsync(x => x.Id == command.RoleId, cancellationToken);
            if (!roleExists)
            {
                _logger.LogWarning("Role not found with ID: {RoleId}", command.RoleId);
                return Result.Failure(HttpStatusCode.NotFound, RoleErrors.RoleNotFound());
            }

            // Check if user role already exists
            var userRoleExists = await userRoleRepo.AnyAsync(
                x => x.UserId == command.UserId && x.RoleId == command.RoleId,
                cancellationToken);
            if (userRoleExists)
            {
                _logger.LogWarning("UserRole already exists for User: {UserId} and Role: {RoleId}", command.UserId, command.RoleId);
                return Result.Failure(HttpStatusCode.BadRequest, UserRoleErrors.UserRoleAlreadyExists());
            }

            var userRole = new UserRole
            {
                UserId = command.UserId,
                RoleId = command.RoleId
            };

            userRoleRepo.Add(userRole);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End CreateUserRoleCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateUserRoleCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
