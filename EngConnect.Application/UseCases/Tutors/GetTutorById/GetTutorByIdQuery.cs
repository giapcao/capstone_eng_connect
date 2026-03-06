using EngConnect.Application.UseCases.Tutors.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Tutors.GetTutorById
{
    public record GetTutorByIdQuery(Guid Id) : BaseQuery<GetTutorResponse>;
}
