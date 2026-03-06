using EngConnect.Application.UseCases.PermissionRoles.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Params;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Application.UseCases.PermissionRoles.GetListPermissionRole;

public record GetListPermissionRoleQuery : BaseQuery<PaginationResult<GetPermissionRoleResponse>>
{
    public Guid? PermissionId { get; set; }
    public Guid? RoleId { get; set; }
}
