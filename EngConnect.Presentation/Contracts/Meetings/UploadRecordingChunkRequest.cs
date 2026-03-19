using Microsoft.AspNetCore.Http;

namespace EngConnect.Presentation.Contracts.Meetings;

public sealed class UploadRecordingChunkRequest
{
    public required IFormFile Chunk { get; init; }
    public required int ChunkIndex { get; init; }
}