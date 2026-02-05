using EngConnect.Application.UseCases.Permissions.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Permissions.GetPermissionById;

public record GetPermissionByIdQuery(Guid Id) : IQuery<GetPermissionResponse>;
