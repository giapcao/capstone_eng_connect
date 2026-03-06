using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseResources.DeleteCourseResource;

public record DeleteCourseResourceCommand(Guid Id) : ICommand;
