using EngConnect.Application.UseCases.CourseModules.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseModules.GetCourseModuleTree;

public record GetCourseModuleTreeQuery(Guid Id) : IQuery<GetCourseModuleTreeResponse>;
