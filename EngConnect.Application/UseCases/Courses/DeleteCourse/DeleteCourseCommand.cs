using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Courses.DeleteCourse;

public record DeleteCourseCommand(Guid Id, Guid? TutorId) : ICommand;
