using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseSessions.DeleteCourseSession;

public record DeleteCourseSessionCommand(Guid Id) : ICommand;
