namespace EngConnect.Application.UseCases.LessonRescheduleRequests.UpdateLessonRescheduleRequest;

public sealed class UpdateLessonRescheduleRequest
{
    public Guid Id { get; set; }
    public string? Status { get; set; }
    public string? TutorNote { get; set; }
}