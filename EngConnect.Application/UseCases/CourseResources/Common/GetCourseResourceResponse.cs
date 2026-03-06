namespace EngConnect.Application.UseCases.CourseResources.Common;

public class GetCourseResourceResponse
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public string? Title { get; set; }
    public string? ResourceType { get; set; }
    public string Url { get; set; } = null!;
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
