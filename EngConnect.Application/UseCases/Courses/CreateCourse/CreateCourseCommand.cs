using EngConnect.Application.UseCases.Courses.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.Files;

namespace EngConnect.Application.UseCases.Courses.CreateCourse;

public class CreateCourseCommand : ICommand<GetCourseResponse>
{
    public required Guid TutorId { get; set; }
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
    
    //260305: Add course categories when creating course
    public Guid[]? CategoryIds { get; set; } = [];

    public decimal Price { get; set; } = 0;
    public string? Currency { get; set; }
    public int NumsSessionInWeek { get; set; } = 0;
    
    public FileUpload? ThumbnailFile { get; set; }
    public FileUpload? DemoVideoFile { get; set; }
    public bool IsCertificate { get; set; } = false;
}
