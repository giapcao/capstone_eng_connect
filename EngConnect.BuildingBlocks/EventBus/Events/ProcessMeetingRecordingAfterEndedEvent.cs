using EngConnect.BuildingBlock.EventBus.Abstraction;

namespace EngConnect.BuildingBlock.EventBus.Events;

public class ProcessMeetingRecordingAfterEndedEvent : EventBase
{
    public Guid LessonId { get; set; }
    public Guid EndedByUserId { get; set; }

    public static ProcessMeetingRecordingAfterEndedEvent Create(Guid lessonId, Guid endedByUserId)
    {
        return new ProcessMeetingRecordingAfterEndedEvent
        {
            LessonId = lessonId,
            EndedByUserId = endedByUserId
        };
    }
}
