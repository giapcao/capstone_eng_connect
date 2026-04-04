using EngConnect.BuildingBlock.EventBus.Abstraction;

namespace EngConnect.BuildingBlock.EventBus.Events;

public class ProcessMeetingRecordingAfterEndedEvent : NotificationEvent
{
    public Guid LessonId { get; set; }
    public Guid EndedByUserId { get; set; }
    public int? TotalChunks { get; set; }

    public static ProcessMeetingRecordingAfterEndedEvent Create(Guid lessonId, Guid endedByUserId, int? totalChunks = null)
    {
        return new ProcessMeetingRecordingAfterEndedEvent
        {
            LessonId = lessonId,
            EndedByUserId = endedByUserId,
            TotalChunks = totalChunks
        };
    }
}
