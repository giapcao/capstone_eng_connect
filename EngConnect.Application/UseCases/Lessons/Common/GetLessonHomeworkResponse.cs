namespace EngConnect.Application.UseCases.Lessons.Common;

public class GetLessonHomeworkResponse
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? ResourceUrl { get; set; }
    public string? SubmissionUrl { get; set; }
    public decimal? Score { get; set; }
    public decimal MaxScore { get; set; }
    public string? TutorFeedback { get; set; }
    public DateTime? AssignedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public DateTime? DueAt { get; set; }
    public string Status { get; set; } = null!;
}
