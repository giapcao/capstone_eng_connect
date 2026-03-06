using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.PermissionRoles.DeletePermissionRole;

public class DeletePermissionRoleCommandHandler : ICommandHandler<DeletePermissionRoleCommand>
{
    private readonly ILogger<DeletePermissionRoleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePermissionRoleCommandHandler(ILogger<DeletePermissionRoleCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeletePermissionRoleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeletePermissionRoleCommandHandler {@Command}", command);
        try
        {
            var permissionRoleRepo = _unitOfWork.GetRepository<PermissionRole, Guid>();

            var permissionRole = await permissionRoleRepo.FindSingleAsync(
                x => x.Id == command.Id, 
                cancellationToken:cancellationToken);
            if (permissionRole == null)
            {
                _logger.LogWarning("PermissionRole not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.BadRequest, PermissionRoleErrors.PermissionRoleNotFound());
            }

            permissionRoleRepo.Delete(permissionRole);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End DeletePermissionRoleCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeletePermissionRoleCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
