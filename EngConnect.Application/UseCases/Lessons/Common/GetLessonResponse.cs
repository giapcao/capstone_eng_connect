namespace EngConnect.Application.UseCases.Lessons.Common;

public class GetLessonResponse
{
    public Guid Id { get; set; }
    
    public Guid CourseId { get; set; }
    
    public Guid TutorId { get; set; }
    
    public Guid StudentId { get; set; }
    
    public Guid EnrollmentId { get; set; }
    
    public Guid? ModuleId { get; set; }
    
    public Guid? SessionId { get; set; }
    
    public DateTime? StartTime { get; set; }
    
    public DateTime? EndTime { get; set; }
    
    public string? Status { get; set; }
    
    public string? MeetingUrl { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}
