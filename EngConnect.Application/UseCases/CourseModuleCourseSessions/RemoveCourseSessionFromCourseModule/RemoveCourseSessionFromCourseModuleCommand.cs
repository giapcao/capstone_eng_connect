using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseModuleCourseSessions.RemoveCourseSessionFromCourseModule;

public record RemoveCourseSessionFromCourseModuleCommand(Guid Id) : ICommand;
