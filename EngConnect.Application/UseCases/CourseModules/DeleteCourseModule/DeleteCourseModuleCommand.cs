using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseModules.DeleteCourseModule;

public record DeleteCourseModuleCommand(Guid Id) : ICommand;
