using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.UserRoles.DeleteUserRole;

public record DeleteUserRoleCommand(Guid Id) : ICommand;
