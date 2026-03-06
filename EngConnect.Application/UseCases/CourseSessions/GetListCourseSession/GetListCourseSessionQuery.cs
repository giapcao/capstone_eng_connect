using EngConnect.Application.UseCases.CourseSessions.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.CourseSessions.GetListCourseSession;

public record GetListCourseSessionQuery : BaseQuery<PaginationResult<GetCourseSessionResponse>>
{
    public Guid? ModuleId { get; set; }
}
