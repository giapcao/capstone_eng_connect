using EngConnect.Application.UseCases.CourseSessions.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseSessions.GetCourseSessionTree;

public record GetCourseSessionTreeQuery(Guid Id) : IQuery<GetCourseSessionTreeResponse>;
