// using System.Text.Json;
// using EngConnect.BuildingBlock.Contracts.Abstraction;
// using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;
// using EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;
// using EngConnect.BuildingBlock.Contracts.Models.Files;
// using EngConnect.BuildingBlock.EventBus.Events;
// using MassTransit;
// using Microsoft.Extensions.Logging;
//
// namespace EngConnect.Infrastructure.RabbitMqConsumer;
//
// public class ProcessChunkByWhisperConsumer : IConsumer<UploadMeetingRecordingChunkEvent>
// {
//     private readonly ILogger<ProcessChunkByWhisperConsumer> _logger;
//     private readonly IRedisService _redisService;
//     private readonly IWhisperApiService _apiService;
//
//     public ProcessChunkByWhisperConsumer(IWhisperApiService apiService, ILogger<ProcessChunkByWhisperConsumer> logger
//         , IRedisService redisService)
//     {
//         _apiService = apiService;
//         _logger = logger;
//         _redisService = redisService;
//     }
//
//     public async Task Consume(ConsumeContext<UploadMeetingRecordingChunkEvent> context)
//     {
//         var eventData = context.Message;
//         _logger.LogInformation("Start ProcessChunkByWhisperConsumer {@EventData}", eventData);
//         try
//         {
//             if (!File.Exists(eventData.TempFilePath))
//             {
//                 _logger.LogWarning("Temp chunk file not found at {TempFilePath}", eventData.TempFilePath);
//                 return;
//             }
//             
//             var fileInfo = new FileInfo(eventData.TempFilePath);
//             await using var stream = new FileStream(eventData.TempFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
//
//             var uploadFile = new FileUpload
//             {
//                 FileName = eventData.OriginalFileName,
//                 ContentType = string.IsNullOrWhiteSpace(eventData.ContentType) ? "video/webm" : eventData.ContentType,
//                 Length = fileInfo.Length,
//                 Content = stream
//             };
//             
//             var jsonResponse = await _apiService.Transcribe(uploadFile);
//             if (jsonResponse == null)
//             {
//                 _logger.LogWarning("jsonResponse returned null");
//                 return;
//             }
//             var result = JsonSerializer.Deserialize<WhisperResponse>(jsonResponse, 
//                 new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
//
//             if (!string.IsNullOrEmpty(result?.Transcription))
//             {
//                 double score = eventData.ChunkIndex; 
//                 await _redisService.SortedSetAddAsync(eventData.LessonId.ToString(), result.Transcription, score);
//             }
//             _logger.LogInformation("End ProcessChunkByWhisperConsumer");
//         }
//         catch (Exception ex)
//         {
//             _logger.LogError(ex,
//                 "Error consuming ProcessChunkByWhisperConsumer for lesson {LessonId}",
//                 eventData.LessonId);
//             throw;
//         }
//         finally
//         {
//             try
//             {
//                 if (File.Exists(eventData.TempFilePath))
//                 {
//                     File.Delete(eventData.TempFilePath);
//                 }
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogWarning(ex, "Failed to delete temp chunk file {TempFilePath}", eventData.TempFilePath);
//             }
//         }
//     }
//
// }