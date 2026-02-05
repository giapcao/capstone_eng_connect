using EngConnect.Application.UseCases.Users.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Users.GetUserById;

public record GetUserByIdQuery(Guid Id) : IQuery<GetUserResponse>;

