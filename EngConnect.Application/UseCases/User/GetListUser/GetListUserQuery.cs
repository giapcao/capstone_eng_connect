using EngConnect.Application.UseCases.User.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.User.GetListUser;

public record GetListUserQuery : BaseQuery<PaginationResult<GetUserResponse>>
{
    public string? Status { get; set; }
}

