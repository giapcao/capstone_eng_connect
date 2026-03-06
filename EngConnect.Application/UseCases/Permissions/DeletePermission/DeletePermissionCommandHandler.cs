using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Permissions.DeletePermission;

public class DeletePermissionCommandHandler : ICommandHandler<DeletePermissionCommand>
{
    private readonly ILogger<DeletePermissionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePermissionCommandHandler(ILogger<DeletePermissionCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeletePermissionCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeletePermissionCommandHandler {@Command}", command);
        try
        {
            var permissionRepo = _unitOfWork.GetRepository<Permission, Guid>();

            var permission = await permissionRepo.FindSingleAsync(
                x => x.Id == command.Id, 
                cancellationToken:cancellationToken);
            if (permission == null)
            {
                _logger.LogWarning("Permission not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, PermissionErrors.PermissionNotFound());
            }

            permissionRepo.Delete(permission);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End DeletePermissionCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeletePermissionCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
