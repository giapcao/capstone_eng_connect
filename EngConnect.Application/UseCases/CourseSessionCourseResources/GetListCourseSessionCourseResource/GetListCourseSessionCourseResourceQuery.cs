using EngConnect.Application.UseCases.CourseSessionCourseResources.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngConnect.Application.UseCases.CourseSessionCourseResources.GetListCourseSessionCourseResource;

public record GetListCourseSessionCourseResourceQuery : BaseQuery<PaginationResult<GetCourseSessionCourseResourceResponse>>
{
    public Guid? CourseSessionId { get; set; }
    
    public Guid? CourseResourceId { get; set; }
}
