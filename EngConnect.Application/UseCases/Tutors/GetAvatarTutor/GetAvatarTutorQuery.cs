using EngConnect.Application.UseCases.Tutors.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Tutors.GetAvatarTutor;

public class GetAvatarTutorQuery : IQuery<GetAvatarTutorResponse>
{
    public required Guid Id { get; set; }
}
