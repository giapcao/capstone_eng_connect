using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseCourseModules.RemoveCourseModuleFromCourse;

public record RemoveCourseModuleFromCourseCommand(Guid Id) : ICommand;
