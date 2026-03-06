using EngConnect.Application.UseCases.Courses.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.Courses.GetListCourse;

public record GetListCourseQuery : BaseQuery<PaginationResult<GetCourseResponse>>
{
    public Guid? TutorId { get; set; }
    public string? Level { get; set; }
    public string? Status { get; set; }
}
