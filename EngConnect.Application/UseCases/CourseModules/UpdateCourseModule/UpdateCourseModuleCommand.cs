using System.Text.Json.Serialization;
using EngConnect.Application.UseCases.CourseModules.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseModules.UpdateCourseModule;

public class UpdateCourseModuleCommand : ICommand<GetCourseModuleResponse>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public Guid? CourseId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? Outcomes { get; set; }
}
