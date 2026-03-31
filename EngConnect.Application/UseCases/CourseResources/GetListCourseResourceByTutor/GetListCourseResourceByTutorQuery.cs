using EngConnect.Application.UseCases.CourseResources.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngConnect.Application.UseCases.CourseResources.GetListCourseResourceByTutor;

public record GetListCourseResourceByTutorQuery : BaseQuery<PaginationResult<GetCourseResourceResponse>>
{
    [BindNever]
    public Guid? TutorId { get; set; }
    public string? ResourceType { get; set; }
    public string? Status { get; set; }
    public Guid? CourseSessionId { get; set; }
}
