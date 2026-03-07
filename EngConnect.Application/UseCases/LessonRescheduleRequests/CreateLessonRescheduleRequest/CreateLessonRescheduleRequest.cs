namespace EngConnect.Application.UseCases.LessonRescheduleRequests.CreateLessonRescheduleRequest;

public sealed class CreateLessonRescheduleRequest
{
    public Guid LessonId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime ProposedStartTime { get; set; }
    public DateTime ProposedEndTime { get; set; }
    public string? TutorNote { get; set; }
}