using EngConnect.Application.UseCases.CourseModules.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngConnect.Application.UseCases.CourseModules.GetListCourseModuleByTutor;

public record GetListCourseModuleByTutorQuery : BaseQuery<PaginationResult<GetCourseModuleResponse>>
{
    [BindNever]
    public Guid? TutorId { get; set; }
    public Guid? CourseId { get; set; }
}
