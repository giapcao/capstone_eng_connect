using EngConnect.Application.UseCases.Tutors.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Tutors.GetCvUrlTutor;

public class GetCvUrlTutorQuery : IQuery<GetCvUrlTutorResponse>
{
    public required Guid Id { get; set; }
}
