using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseSessionCourseResources.RemoveCourseResourceFromCourseSession;

public record RemoveCourseResourceFromCourseSessionCommand(Guid Id) : ICommand;
