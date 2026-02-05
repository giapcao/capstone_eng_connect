using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.LessonRecords.UpdateLessonRecord;

public class UpdateLessonRecordCommand : ICommand
{
    public Guid Id { get; set; }

    public Guid LessonId { get; set; }

    public string RecordUrl { get; set; } = null!;

    public int? DurationSeconds { get; set; }

    public DateTime? RecordingStartedAt { get; set; }

    public DateTime? RecordingEndedAt { get; set; }
}
