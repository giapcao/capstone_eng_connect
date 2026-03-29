using EngConnect.Application.UseCases.CourseModules.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseModules.GetCourseModuleById;

public record GetCourseModuleByIdQuery(Guid Id) : IQuery<GetCourseModuleDetailResponse>;
