using System.Text.Json.Serialization;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseModules.UpdateCourseModule;

public class UpdateCourseModuleCommand : ICommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Outcomes { get; set; }
}
