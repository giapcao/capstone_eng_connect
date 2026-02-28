using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Courses.CreateCourse;

public class CreateCourseCommand : ICommand
{
    public required Guid TutorId { get; set; }
    public Guid? ParentCourseId { get; set; }
    public required string Title { get; set; }
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? Outcomes { get; set; }
    public string? Level { get; set; }
    
    /// <summary>
    /// Estimated time to complete the course in minutes
    /// </summary>
    public int EstimatedTime { get; set; } = 0;
    
    /// <summary>
    /// Estimated time per lesson in minutes
    /// </summary>
    public int EstimatedTimeLesson { get; set; }

    public decimal Price { get; set; } = 0;
    public string? Currency { get; set; }
    public int NumberOfSessions { get; set; } = 0;
    public int NumsSessionInWeek { get; set; } = 0;
    public string? ThumbnailUrl { get; set; }
    public string? DemoVideoUrl { get; set; }
    public bool IsCertificate { get; set; } = false;
}
