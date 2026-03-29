using EngConnect.Application.UseCases.CourseModuleCourseSessions.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngConnect.Application.UseCases.CourseModuleCourseSessions.GetListCourseModuleCourseSession;

public record GetListCourseModuleCourseSessionQuery : BaseQuery<PaginationResult<GetCourseModuleCourseSessionResponse>>
{
    public Guid? CourseModuleId { get; set; }
    
    public Guid? CourseSessionId { get; set; }
}
