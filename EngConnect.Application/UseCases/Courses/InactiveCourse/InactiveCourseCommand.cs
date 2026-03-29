using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Courses.InactiveCourse;

public record InactiveCourseCommand(Guid Id, Guid? TutorId) : ICommand;
