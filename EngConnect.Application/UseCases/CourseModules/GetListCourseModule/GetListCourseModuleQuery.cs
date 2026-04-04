using EngConnect.Application.UseCases.CourseModules.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngConnect.Application.UseCases.CourseModules.GetListCourseModule;

public record GetListCourseModuleQuery : BaseQuery<PaginationResult<GetCourseModuleResponse>>
{
    [BindNever]
    public Guid? TutorId { get; set; }
    public Guid? CourseId { get; set; }
}
