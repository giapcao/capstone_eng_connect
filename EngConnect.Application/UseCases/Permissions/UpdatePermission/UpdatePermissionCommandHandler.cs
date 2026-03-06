using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Permissions.UpdatePermission;

public class UpdatePermissionCommandHandler : ICommandHandler<UpdatePermissionCommand>
{
    private readonly ILogger<UpdatePermissionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePermissionCommandHandler(ILogger<UpdatePermissionCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdatePermissionCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdatePermissionCommandHandler {@Command}", command);
        try
        {
            var permissionRepo = _unitOfWork.GetRepository<Permission, Guid>();

            var permission = await permissionRepo.FindSingleAsync(
                x=> x.Id == command.Id, 
                cancellationToken: cancellationToken);
            if (permission == null)
            {
                _logger.LogWarning("Permission not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, PermissionErrors.PermissionNotFound());
            }

            // Check if new code already exists (except current permission)
            var codeExists = await permissionRepo.AnyAsync(
                x => x.Code == command.Code && x.Id != command.Id, 
                cancellationToken);
            if (codeExists)
            {
                _logger.LogWarning("Permission code already exists: {Code}", command.Code);
                return Result.Failure(HttpStatusCode.BadRequest, PermissionErrors.PermissionAlreadyExists());
            }

            permission.Code = command.Code;
            permission.Description = command.Description;
            permissionRepo.Update(permission);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdatePermissionCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdatePermissionCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
