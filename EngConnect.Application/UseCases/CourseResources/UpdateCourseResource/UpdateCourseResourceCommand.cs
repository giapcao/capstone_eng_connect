using System.Text.Json.Serialization;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseResources.UpdateCourseResource;

public class UpdateCourseResourceCommand : ICommand
{
    [JsonIgnore]
    public Guid Id { get; set; }

    [JsonIgnore]
    public Guid? TutorId { get; set; }

    public string? Title { get; set; }
    public string? ResourceType { get; set; }
    public string? Url { get; set; }
    public string? Status { get; set; }
}
