using EngConnect.BuildingBlock.EventBus.Abstraction;

namespace EngConnect.BuildingBlock.EventBus.Events;

public class UploadMeetingRecordingChunkEvent : EventBase
{
    public Guid LessonId { get; set; }
    public Guid UserId { get; set; }
    public long ChunkTimestamp { get; set; }
    public string TempFilePath { get; set; } = null!;
    public string OriginalFileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;

    public static UploadMeetingRecordingChunkEvent Create(
        Guid lessonId,
        Guid userId,
        long chunkTimestamp,
        string tempFilePath,
        string originalFileName,
        string contentType)
    {
        return new UploadMeetingRecordingChunkEvent
        {
            LessonId = lessonId,
            UserId = userId,
            ChunkTimestamp = chunkTimestamp,
            TempFilePath = tempFilePath,
            OriginalFileName = originalFileName,
            ContentType = contentType
        };
    }
}
