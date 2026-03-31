namespace EngConnect.Application.UseCases.LessonRecords.Common;

public class GetLessonRecordResponse
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    public string RecordUrl { get; set; } = null!;
    public int? DurationSeconds { get; set; }
    public DateTime? RecordingStartedAt { get; set; }
    public DateTime? RecordingEndedAt { get; set; }
    public LessonRecordLessonScriptResponse? LessonScript { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class LessonRecordLessonScriptResponse
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
