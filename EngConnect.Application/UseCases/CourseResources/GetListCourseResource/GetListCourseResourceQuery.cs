using EngConnect.Application.UseCases.CourseResources.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.CourseResources.GetListCourseResource;

public record GetListCourseResourceQuery : BaseQuery<PaginationResult<GetCourseResourceResponse>>
{
    public Guid? SessionId { get; set; }
    public string? ResourceType { get; set; }
    public string? Status { get; set; }
}
