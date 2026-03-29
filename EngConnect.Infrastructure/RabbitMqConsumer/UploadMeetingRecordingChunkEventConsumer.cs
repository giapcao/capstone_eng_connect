using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.EventBus.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EngConnect.Infrastructure.RabbitMqConsumer;

public class UploadMeetingRecordingChunkEventConsumer : IConsumer<UploadMeetingRecordingChunkEvent>
{
    private readonly IDriveService _driveService;
    private readonly ILogger<UploadMeetingRecordingChunkEventConsumer> _logger;

    public UploadMeetingRecordingChunkEventConsumer(
        IDriveService driveService,
        ILogger<UploadMeetingRecordingChunkEventConsumer> logger)
    {
        _driveService = driveService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UploadMeetingRecordingChunkEvent> context)
    {
        _logger.LogInformation("Start UploadMeetingRecordingChunkEventConsumer {@EventData}", context.Message);
        var eventData = context.Message;

        try
        {
            if (!File.Exists(eventData.TempFilePath))
            {
                _logger.LogWarning("Temp chunk file not found at {TempFilePath}", eventData.TempFilePath);
                return;
            }

            var fileInfo = new FileInfo(eventData.TempFilePath);
            await using var stream = new FileStream(eventData.TempFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            var uploadFile = new FileUpload
            {
                FileName = eventData.OriginalFileName,
                ContentType = string.IsNullOrWhiteSpace(eventData.ContentType) ? "video/webm" : eventData.ContentType,
                Length = fileInfo.Length,
                Content = stream
            };

            await _driveService.UploadMeetingChunkAsync(
                eventData.LessonId,
                eventData.ChunkIndex,
                uploadFile,
                context.CancellationToken);

            _logger.LogInformation(
                "Uploaded meeting chunk {ChunkIndex} for lesson {LessonId} to Drive",
                eventData.ChunkIndex,
                eventData.LessonId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error consuming UploadMeetingRecordingChunkEvent for LessonId {LessonId}, ChunkIndex {ChunkIndex}",
                eventData.LessonId,
                eventData.ChunkIndex);
            throw;
        }
        finally
        {
            try
            {
                if (File.Exists(eventData.TempFilePath))
                {
                    File.Delete(eventData.TempFilePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to delete temp chunk file {TempFilePath}", eventData.TempFilePath);
            }
        }

        _logger.LogInformation("End UploadMeetingRecordingChunkEventConsumer");
    }
}
