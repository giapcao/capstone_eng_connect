using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Permissions.DeletePermission;

public record DeletePermissionCommand(Guid Id) : ICommand;
