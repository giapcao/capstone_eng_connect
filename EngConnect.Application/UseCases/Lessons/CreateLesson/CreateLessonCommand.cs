using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Lessons.CreateLesson;

public class CreateLessonCommand : ICommand
{ 
    public Guid StudentId { get; set; }
    
    public Guid EnrollmentId { get; set; }
    
    public Guid? ModuleId { get; set; }
    
    public Guid? SessionId { get; set; }
    
    public DateTime? StartTime { get; set; }
    
    public DateTime? EndTime { get; set; }
    
    public string? MeetingUrl { get; set; }
}
