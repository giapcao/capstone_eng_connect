using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Roles.DeleteRole;

public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand>
{
    private readonly ILogger<DeleteRoleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoleCommandHandler(ILogger<DeleteRoleCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeleteRoleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteRoleCommandHandler {@Command}", command);
        try
        {
            var roleRepo = _unitOfWork.GetRepository<Role, Guid>();

            var role = await roleRepo.FindFirstAsync(
                x => x.Id == command.Id, 
                cancellationToken:cancellationToken);
            if (role == null)
            {
                _logger.LogWarning("Role not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, RoleErrors.RoleNotFound());
            }

            roleRepo.Delete(role);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End DeleteRoleCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteRoleCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
