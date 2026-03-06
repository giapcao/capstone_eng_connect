using EngConnect.Application.UseCases.Tutors.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.Tutors.GetListTutor
{
    public record GetListTutorQuery(string? Status, string? VerifiedStatus) : BaseQuery<PaginationResult<GetTutorResponse>>;

}
