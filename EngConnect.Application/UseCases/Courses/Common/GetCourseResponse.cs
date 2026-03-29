using EngConnect.Application.UseCases.CourseModules.Common;

namespace EngConnect.Application.UseCases.Courses.Common;

public class GetCourseResponse
{
    public Guid Id { get; set; }
    public Guid TutorId { get; set; }
    public Guid? ParentCourseId { get; set; }
    public string Title { get; set; } = null!;
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public string? Outcomes { get; set; }
    public string? Level { get; set; }
    public TimeSpan? EstimatedTime { get; set; }
    public TimeSpan? EstimatedTimeLesson { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public int? NumberOfSessions { get; set; }
    public int? NumsSessionInWeek { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? DemoVideoUrl { get; set; }
    public List<GetCourseCategoryResponseInCourse> CourseCategories { get; set; } = [];
    public int? NumberOfEnrollment { get; set; }
    public decimal? RatingAverage { get; set; }
    public int? RatingCount { get; set; }
    public string? Status { get; set; }
    public bool? IsCertificate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}




