using EngConnect.Application.UseCases.UserRoles.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.UserRoles.GetUserRoleById;

public record GetUserRoleByIdQuery(Guid Id) : IQuery<GetUserRoleResponse>;
