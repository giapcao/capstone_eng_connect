using EngConnect.Application.UseCases.CourseModules.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.CourseModules.GetListCourseModule;

public record GetListCourseModuleQuery : BaseQuery<PaginationResult<GetCourseModuleResponse>>
{
    public Guid? CourseId { get; set; }
}
