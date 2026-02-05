using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.PermissionRoles.DeletePermissionRole;

public record DeletePermissionRoleCommand(Guid Id) : ICommand;
