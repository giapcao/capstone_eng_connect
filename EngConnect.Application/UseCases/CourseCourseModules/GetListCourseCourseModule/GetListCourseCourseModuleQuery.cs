using EngConnect.Application.UseCases.CourseCourseModules.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngConnect.Application.UseCases.CourseCourseModules.GetListCourseCourseModule;

public record GetListCourseCourseModuleQuery : BaseQuery<PaginationResult<GetCourseCourseModuleResponse>>
{
    public Guid? CourseId { get; set; }
    
    public Guid? CourseModuleId { get; set; }
}
