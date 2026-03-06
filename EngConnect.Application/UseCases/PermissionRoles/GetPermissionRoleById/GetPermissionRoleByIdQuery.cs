using EngConnect.Application.UseCases.PermissionRoles.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.PermissionRoles.GetPermissionRoleById;

public record GetPermissionRoleByIdQuery(Guid Id) : IQuery<GetPermissionRoleResponse>;
