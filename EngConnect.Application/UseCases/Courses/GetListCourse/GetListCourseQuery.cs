using EngConnect.Application.UseCases.Courses.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngConnect.Application.UseCases.Courses.GetListCourse;

public record GetListCourseQuery : BaseQuery<PaginationResult<GetCourseResponse>>
{
    [BindNever]
    public Guid? TutorId { get; set; }
    [BindNever]
    public Guid? StudentId { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Level { get; set; }
    public string? Status { get; set; }
}
