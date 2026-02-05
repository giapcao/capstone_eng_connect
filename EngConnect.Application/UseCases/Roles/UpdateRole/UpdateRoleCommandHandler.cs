using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Roles.UpdateRole;

public class UpdateRoleCommandHandler : ICommandHandler<UpdateRoleCommand>
{
    private readonly ILogger<UpdateRoleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoleCommandHandler(ILogger<UpdateRoleCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateRoleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateRoleCommandHandler {@Command}", command);
        try
        {
            var roleRepo = _unitOfWork.GetRepository<Role, Guid>();

            var role = await roleRepo.FindSingleAsync(
                x => x.Id == command.Id, 
                cancellationToken: cancellationToken);
            if (role == null)
            {
                _logger.LogWarning("Role not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, new Error("RoleNotFound", "Vai trò không tồn tại"));
            }

            // Check if new code already exists (except current role)
            var codeExists = await roleRepo.AnyAsync(
                x => x.Code == command.Code && x.Id != command.Id, 
                cancellationToken);
            if (codeExists)
            {
                _logger.LogWarning("Role code already exists: {Code}", command.Code);
                return Result.Failure(HttpStatusCode.BadRequest, RoleErrors.RoleAlreadyExists());
            }

            role.Code = command.Code;
            role.Description = command.Description;
            roleRepo.Update(role);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdateRoleCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateRoleCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
