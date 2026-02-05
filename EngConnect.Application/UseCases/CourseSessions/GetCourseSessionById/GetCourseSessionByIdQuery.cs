using EngConnect.Application.UseCases.CourseSessions.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseSessions.GetCourseSessionById;

public record GetCourseSessionByIdQuery(Guid Id) : IQuery<GetCourseSessionResponse>;
