namespace EngConnect.Application.UseCases.LessonRecords.Common;

public class GetLessonRecordResponse
{
    public Guid Id { get; set; }

    public Guid LessonId { get; set; }

    public string RecordUrl { get; set; } = null!;

    public int? DurationSeconds { get; set; }

    public DateTime? RecordingStartedAt { get; set; }

    public DateTime? RecordingEndedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
