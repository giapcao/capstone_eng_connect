using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.LessonRecords.CreateLessonRecord;

public class CreateLessonRecordCommand : ICommand
{
    public Guid LessonId { get; set; }

    public string RecordUrl { get; set; } = null!;

    public int? DurationSeconds { get; set; }

    public DateTime? RecordingStartedAt { get; set; }

    public DateTime? RecordingEndedAt { get; set; }
}
