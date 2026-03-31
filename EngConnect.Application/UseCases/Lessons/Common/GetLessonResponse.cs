namespace EngConnect.Application.UseCases.Lessons.Common;

public class GetLessonResponse
{
    public Guid Id { get; set; }
    public Guid TutorId { get; set; }
    public Guid StudentId { get; set; }
    public Guid EnrollmentId { get; set; }
    public Guid? SessionId { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? Status { get; set; }
    public string? MeetingUrl { get; set; }
    public string? MeetingStatus { get; set; }
    public DateTime? MeetingStartedAt { get; set; }
    public DateTime? MeetingEndedAt { get; set; }
    public LessonRecordSummaryResponse? LessonRecord { get; set; }
    public LessonScriptSummaryResponse? LessonScript { get; set; }
    public List<GetLessonHomeworkResponse> LessonHomeworks { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class LessonRecordSummaryResponse
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    public string RecordUrl { get; set; } = null!;
    public int? DurationSeconds { get; set; }
    public DateTime? RecordingStartedAt { get; set; }
    public DateTime? RecordingEndedAt { get; set; }
}

public class LessonScriptSummaryResponse
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    public Guid RecordId { get; set; }
    public string? Language { get; set; }
    public string? FullText { get; set; }
    public string? SummarizeText { get; set; }
    public string? LessonOutcome { get; set; }
    public decimal? CoveragePercent { get; set; }
}
