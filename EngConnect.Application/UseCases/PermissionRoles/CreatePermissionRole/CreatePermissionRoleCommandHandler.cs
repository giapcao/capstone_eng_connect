using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole;

public class CreatePermissionRoleCommandHandler : ICommandHandler<CreatePermissionRoleCommand>
{
    private readonly ILogger<CreatePermissionRoleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePermissionRoleCommandHandler(ILogger<CreatePermissionRoleCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreatePermissionRoleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreatePermissionRoleCommandHandler {@Command}", command);
        try
        {
            var permissionRoleRepo = _unitOfWork.GetRepository<PermissionRole, Guid>();
            var permissionRepo = _unitOfWork.GetRepository<Permission, Guid>();
            var roleRepo = _unitOfWork.GetRepository<Role, Guid>();

            // Check if permission exists
            var permissionExists = await permissionRepo.AnyAsync(x => x.Id == command.PermissionId, cancellationToken);
            if (!permissionExists)
            {
                _logger.LogWarning("Permission not found with ID: {PermissionId}", command.PermissionId);
                return Result.Failure(HttpStatusCode.BadRequest, PermissionErrors.PermissionNotFound());
            }

            // Check if role exists
            var roleExists = await roleRepo.AnyAsync(x => x.Id == command.RoleId, cancellationToken);
            if (!roleExists)
            {
                _logger.LogWarning("Role not found with ID: {RoleId}", command.RoleId);
                return Result.Failure(HttpStatusCode.BadRequest, RoleErrors.RoleNotFound());
            }

            // Check if permission role already exists
            var permissionRoleExists = await permissionRoleRepo.AnyAsync(
                x => x.PermissionId == command.PermissionId && x.RoleId == command.RoleId,
                cancellationToken);
            if (permissionRoleExists)
            {
                _logger.LogWarning("PermissionRole already exists for Permission: {PermissionId} and Role: {RoleId}", command.PermissionId, command.RoleId);
                return Result.Failure(HttpStatusCode.BadRequest, PermissionRoleErrors.PermissionRoleAlreadyExists());
            }

            var permissionRole = new PermissionRole
            {
                PermissionId = command.PermissionId,
                RoleId = command.RoleId
            };

            permissionRoleRepo.Add(permissionRole);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End CreatePermissionRoleCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreatePermissionRoleCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
