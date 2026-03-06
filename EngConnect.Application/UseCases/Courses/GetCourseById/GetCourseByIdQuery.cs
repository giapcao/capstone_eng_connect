using EngConnect.Application.UseCases.Courses.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Courses.GetCourseById;

public record GetCourseByIdQuery(Guid Id) : IQuery<GetCourseResponseDetail>;
