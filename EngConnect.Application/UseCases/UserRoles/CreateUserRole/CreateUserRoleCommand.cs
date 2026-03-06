using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.UserRoles.CreateUserRole;

public class CreateUserRoleCommand : ICommand
{
    public required Guid UserId { get; set; }
    public required Guid RoleId { get; set; }
}
