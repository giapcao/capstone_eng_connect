using System.Text.Json.Serialization;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseModules.CreateCourseModule;

public class CreateCourseModuleCommand : ICommand
{
    [JsonIgnore]
    public Guid? TutorId { get; set; }
    public required Guid CourseId { get; set; }
    
    public List<AddNewCourseModule>? NewCourseModules { get; set; } = [];

    public List<CourseModuleIdExist>? CourseModuleIdExists { get; set; } = [];
}


public class CourseModuleIdExist
{
    public Guid CourseModuleId { get; set; }
    public int? ModuleNumber { get; set; }
}

public class AddNewCourseModule
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Outcomes { get; set; }
    public int? ModuleNumber { get; set; }
}
