using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Roles.CreateRole;

public class CreateRoleCommand : ICommand
{
    public required string Code { get; set; }
    public string? Description { get; set; }
}
