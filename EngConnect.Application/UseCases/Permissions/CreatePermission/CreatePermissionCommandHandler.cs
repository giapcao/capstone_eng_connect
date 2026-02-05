using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Permissions.CreatePermission;

public class CreatePermissionCommandHandler : ICommandHandler<CreatePermissionCommand>
{
    private readonly ILogger<CreatePermissionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePermissionCommandHandler(ILogger<CreatePermissionCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreatePermissionCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreatePermissionCommandHandler {@Command}", command);
        try
        {
            var permissionRepo = _unitOfWork.GetRepository<Permission, Guid>();

            // Check if permission code already exists
            var permissionExists = await permissionRepo.AnyAsync(x => x.Code == command.Code, cancellationToken);
            if (permissionExists)
            {
                _logger.LogWarning("Permission already exists with code: {Code}", command.Code);
                return Result.Failure(HttpStatusCode.BadRequest, PermissionErrors.PermissionAlreadyExists());
            }

            var permission = new Permission
            {
                Code = command.Code,
                Description = command.Description
            };

            permissionRepo.Add(permission);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End CreatePermissionCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreatePermissionCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
