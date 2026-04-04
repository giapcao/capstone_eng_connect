using Microsoft.AspNetCore.Http;

namespace EngConnect.Application.UseCases.Courses.Common;

public class CreateCourseRequest
{
    public Guid? ParentCourseId { get; set; }
    public required string Title { get; set; }
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? Outcomes { get; set; }
    public string? Level { get; set; }
    
    
    /// <summary>
    /// Estimated time per lesson in minutes
    /// </summary>
    public int EstimatedTimeLesson { get; set; }
    
    public Guid[]? CategoryIds { get; set; } = [];
    public decimal Price { get; set; } = 0;
    public string? Currency { get; set; }
    public int NumsSessionInWeek { get; set; } = 0;
    public bool IsCertificate { get; set; } = false;
    
    public IFormFile? ThumbnailFile { get; set; }
    public string? ThumbnailFileName { get; set; }
    public IFormFile? DemoVideoFile { get; set; }
    public string? DemoVideoFileName { get; set; }
}
