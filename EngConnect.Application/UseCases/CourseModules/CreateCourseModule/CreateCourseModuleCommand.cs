using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseModules.CreateCourseModule;

public class CreateCourseModuleCommand : ICommand
{
    public required Guid CourseId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Outcomes { get; set; }
    public int? ModuleNumber { get; set; }
}
