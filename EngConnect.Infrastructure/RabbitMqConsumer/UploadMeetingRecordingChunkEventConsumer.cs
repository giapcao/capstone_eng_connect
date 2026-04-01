using System.Text.Json;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;
using EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.EventBus.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EngConnect.Infrastructure.RabbitMqConsumer;

public class UploadMeetingRecordingChunkEventConsumer : IConsumer<UploadMeetingRecordingChunkEvent>
{
    private readonly IDriveService _driveService;
    private readonly ILogger<UploadMeetingRecordingChunkEventConsumer> _logger;
    private readonly IWhisperApiService _apiService;
    private readonly IRedisService _redisService;

    public UploadMeetingRecordingChunkEventConsumer(
        IDriveService driveService,
        ILogger<UploadMeetingRecordingChunkEventConsumer> logger,
        IWhisperApiService  apiService,
        IRedisService redisService)
    {
        _driveService = driveService;
        _logger = logger;
        _apiService = apiService;
        _redisService = redisService;
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
            await using (var stream = new FileStream(eventData.TempFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var uploadFile = new FileUpload
                {
                    FileName = eventData.OriginalFileName,
                    ContentType = string.IsNullOrWhiteSpace(eventData.ContentType) ? "video/webm" : eventData.ContentType,
                    Length = fileInfo.Length,
                    Content = stream
                };
                var jsonResponse = await _apiService.Transcribe(uploadFile);
                if (jsonResponse == null)
                {
                    _logger.LogWarning("jsonResponse returned null");
                    return;
                }
                var result = JsonSerializer.Deserialize<WhisperResponse>(jsonResponse, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (!string.IsNullOrEmpty(result?.Transcription))
                {
                    double score = eventData.ChunkTimestamp;
                    await _redisService.SortedSetAddAsync(eventData.LessonId.ToString(), result.Transcription, score);
                }
            }

            await using (var stream = new FileStream(eventData.TempFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var uploadFile = new FileUpload
                {
                    FileName = eventData.OriginalFileName,
                    ContentType = string.IsNullOrWhiteSpace(eventData.ContentType) ? "video/webm" : eventData.ContentType,
                    Length = fileInfo.Length,
                    Content = stream
                };
                await _driveService.UploadMeetingChunkAsync(
                    eventData.LessonId,
                    eventData.ChunkTimestamp,
                    uploadFile,
                    context.CancellationToken);

            }
          
            _logger.LogInformation(
                "Uploaded meeting chunk {ChunkTimestamp} for lesson {LessonId} to Drive",
                eventData.ChunkTimestamp,
                eventData.LessonId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error consuming UploadMeetingRecordingChunkEvent for LessonId {LessonId}, ChunkTimestamp {ChunkTimestamp}",
                eventData.LessonId,
                eventData.ChunkTimestamp);
            throw;
        }
        // finally
        // {
        //     try
        //     {
        //         if (File.Exists(eventData.TempFilePath))
        //         {
        //             File.Delete(eventData.TempFilePath);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogWarning(ex, "Failed to delete temp chunk file {TempFilePath}", eventData.TempFilePath);
        //     }
        // }
        _logger.LogInformation("End UploadMeetingRecordingChunkEventConsumer");
    }
}
