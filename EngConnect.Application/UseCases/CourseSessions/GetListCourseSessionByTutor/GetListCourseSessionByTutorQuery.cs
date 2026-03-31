using EngConnect.Application.UseCases.CourseSessions.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngConnect.Application.UseCases.CourseSessions.GetListCourseSessionByTutor;

public record GetListCourseSessionByTutorQuery : BaseQuery<PaginationResult<GetCourseSessionResponse>>
{
    [BindNever]
    public Guid? TutorId { get; set; }
    public Guid? CourseModuleId { get; set; }
}
