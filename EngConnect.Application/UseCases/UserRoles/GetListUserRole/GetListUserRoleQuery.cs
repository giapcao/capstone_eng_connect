using EngConnect.Application.UseCases.UserRoles.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.UserRoles.GetListUserRole;

public record GetListUserRoleQuery : BaseQuery<PaginationResult<GetUserRoleResponse>>
{
    public Guid? UserId { get; set; }
    public Guid? RoleId { get; set; }
}
