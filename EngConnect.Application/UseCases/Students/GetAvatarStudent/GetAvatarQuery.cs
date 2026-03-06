using EngConnect.Application.UseCases.Students.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Students.GetAvatarStudent;

public class GetAvatarQuery : IQuery<GetAvatarResponse>
{
    public required Guid Id {get;set;}
}