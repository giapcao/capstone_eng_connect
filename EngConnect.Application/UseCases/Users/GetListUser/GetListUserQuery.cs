using EngConnect.Application.UseCases.Users.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.Users.GetListUser;

public record GetListUserQuery : BaseQuery<PaginationResult<GetUserResponse>>
{
    public string? Status { get; set; }
    
    //Implement flag to show user roles and permissions
}

