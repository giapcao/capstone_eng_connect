using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Roles.DeleteRole;

public record DeleteRoleCommand(Guid Id) : ICommand;
