using EngConnect.Application.UseCases.Roles.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Params;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Shared;

namespace EngConnect.Application.UseCases.Roles.GetListRole;

public record GetListRoleQuery :BaseQuery<PaginationResult<GetRoleResponse>>
{
    public string? Code { get; set; }
}
