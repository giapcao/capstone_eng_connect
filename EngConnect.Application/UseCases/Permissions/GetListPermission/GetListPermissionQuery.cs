using EngConnect.Application.UseCases.Permissions.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Params;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Application.UseCases.Permissions.GetListPermission;

public record GetListPermissionQuery : BaseQuery<PaginationResult<GetPermissionResponse>>
{
    public string? Code { get; set; }
}
