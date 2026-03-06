using EngConnect.Application.UseCases.Roles.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Roles.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IQuery<GetRoleResponse>;
