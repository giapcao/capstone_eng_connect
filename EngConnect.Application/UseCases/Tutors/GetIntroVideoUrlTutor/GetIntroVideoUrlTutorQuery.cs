using EngConnect.Application.UseCases.Tutors.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Tutors.GetIntroVideoUrlTutor;

public class GetIntroVideoUrlTutorQuery : IQuery<GetIntroVideoUrlTutorResponse>
{
    public required Guid Id { get; set; }
}
