using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Permissions.CreatePermission;

public class CreatePermissionCommand : ICommand
{
    public required string Code { get; set; }
    public string? Description { get; set; }
}
