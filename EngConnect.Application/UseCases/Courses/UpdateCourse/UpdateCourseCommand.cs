using System.Text.Json.Serialization;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Courses.UpdateCourse;

public class UpdateCourseCommand : ICommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? Outcomes { get; set; }
    public string? Level { get; set; }
    
    /// <summary>
    /// Estimated time to complete lesson in minutes
    /// </summary>
    public int EstimatedTime { get; set; } = 0;

    public int EstimatedTimeLesson { get; set; } = 0;
    public decimal Price { get; set; }
    public string? Currency { get; set; }
    public int NumberOfSessions { get; set; } = 0;
    public int NumsSessionInWeek { get; set; } = 0;
    public string? ThumbnailUrl { get; set; }
    public string? DemoVideoUrl { get; set; }
    public string? Status { get; set; }
    public bool IsCertificate { get; set; } = false;
}
