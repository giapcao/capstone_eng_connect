using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Lessons.UpdateLesson;

public class UpdateLessonCommand : ICommand
{
    public Guid Id { get; set; }
    public Guid TutorId { get; set; }
    public Guid StudentId { get; set; }
    public Guid EnrollmentId { get; set; }
    public Guid? SessionId { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? MeetingUrl { get; set; }
}
