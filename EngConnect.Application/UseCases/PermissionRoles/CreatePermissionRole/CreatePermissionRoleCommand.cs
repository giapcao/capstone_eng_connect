using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole;

public class CreatePermissionRoleCommand : ICommand
{
    public required Guid PermissionId { get; set; }
    public required Guid RoleId { get; set; }
}
