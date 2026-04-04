using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.Files;

namespace EngConnect.Application.UseCases.Meetings.UploadRecordingChunk;

public class UploadRecordingChunkCommand : ICommand
{
    public required Guid LessonId { get; set; }

    public required Guid UserId { get; set; }

    public required long ChunkTimestamp { get; set; }

    public required FileUpload File { get; set; }
}
