using EngConnect.Application.UseCases.CourseResources.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseResources.GetCourseResourceById;

public record GetCourseResourceByIdQuery(Guid Id) : IQuery<GetCourseResourceResponse>;
