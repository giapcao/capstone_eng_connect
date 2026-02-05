using EngConnect.Application.UseCases.Students.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.Students.GetListStudents;

public record GetListStudentQuery : BaseQuery<PaginationResult<GetStudentResponse>>
{
    public string? Status { get; set; }
}