using System.Text.RegularExpressions;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.EventBus.Events;
using EngConnect.Domain.Persistence.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EngConnect.Infrastructure.RabbitMqConsumer;

public class ProcessMeetingRecordingAfterEndedEventConsumer : IConsumer<ProcessMeetingRecordingAfterEndedEvent>
{
    private const int DefaultChunkSeconds = 30;
    private const string LessonRecordPrefix = "lesson-records";
    private readonly IAwsStorageService _awsStorageService;
    private readonly IDriveService _driveService;
    private readonly ILogger<ProcessMeetingRecordingAfterEndedEventConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ProcessMeetingRecordingAfterEndedEventConsumer(
        IAwsStorageService awsStorageService,
        IDriveService driveService,
        IUnitOfWork unitOfWork,
        ILogger<ProcessMeetingRecordingAfterEndedEventConsumer> logger)
    {
        _awsStorageService = awsStorageService;
        _driveService = driveService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProcessMeetingRecordingAfterEndedEvent> context)
    {
        var eventData = context.Message;
        _logger.LogInformation("Start ProcessMeetingRecordingAfterEndedEventConsumer {@EventData}", eventData);

        var tempWorkingFolder = Path.Combine(
            Path.GetTempPath(),
            "engconnect",
            "meeting-recordings",
            $"lesson-{eventData.LessonId}",
            $"merge-{Guid.NewGuid():N}");

        try
        {
            var chunks = await _driveService.GetMeetingChunksAsync(eventData.LessonId, context.CancellationToken);
            if (chunks.Count == 0)
            {
                _logger.LogInformation("No recording chunks found for lesson {LessonId}", eventData.LessonId);
                return;
            }

            var orderedChunks = chunks
                .OrderBy(x => GetChunkOrder(x.StoredFileName))
                .ThenBy(x => x.StoredFileName)
                .ToList();

            var firstChunk = orderedChunks[0];
            var extension = Path.GetExtension(firstChunk.StoredFileName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".webm";
            }

            Directory.CreateDirectory(tempWorkingFolder);
            var mergedFileName = $"lesson-{eventData.LessonId}-{DateTime.UtcNow:yyyyMMddHHmmss}{extension}";
            var mergedFilePath = Path.Combine(tempWorkingFolder, mergedFileName);

            await using (var mergedOutputStream = new FileStream(mergedFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                foreach (var chunk in orderedChunks)
                {
                    await using var chunkStream = await _driveService.DownloadFileAsync(chunk.RelativePath, context.CancellationToken);
                    await chunkStream.CopyToAsync(mergedOutputStream, context.CancellationToken);
                }
            }

            var mergedFileInfo = new FileInfo(mergedFilePath);
            await using var uploadStream = new FileStream(mergedFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            var mergedFile = new FileUpload
            {
                FileName = mergedFileName,
                ContentType = string.IsNullOrWhiteSpace(firstChunk.ContentType) ? "video/webm" : firstChunk.ContentType,
                Length = mergedFileInfo.Length,
                Content = uploadStream
            };

            var awsUploadResult = await _awsStorageService.UploadFileAsync(
                mergedFile,
                eventData.EndedByUserId,
                LessonRecordPrefix,
                context.CancellationToken);

            var lessonRepo = _unitOfWork.GetRepository<Lesson, Guid>();
            var lesson = await lessonRepo.FindByIdAsync(eventData.LessonId, cancellationToken: context.CancellationToken);
            if (lesson == null)
            {
                _logger.LogWarning("Lesson not found when processing meeting recording for lesson {LessonId}", eventData.LessonId);
                return;
            }

            var lessonRecordRepo = _unitOfWork.GetRepository<LessonRecord, Guid>();
            var lessonRecord = await lessonRecordRepo.FindFirstAsync(
                x => x.LessonId == lesson.Id,
                tracking: true,
                cancellationToken: context.CancellationToken);

            var durationSeconds = CalculateDurationSeconds(lesson, orderedChunks.Count);

            if (lessonRecord == null)
            {
                lessonRecord = new LessonRecord
                {
                    LessonId = lesson.Id,
                    RecordUrl = awsUploadResult.Url,
                    DurationSeconds = durationSeconds,
                    RecordingStartedAt = lesson.MeetingStartedAt,
                    RecordingEndedAt = lesson.MeetingEndedAt
                };

                lessonRecordRepo.Add(lessonRecord);
            }
            else
            {
                lessonRecord.RecordUrl = awsUploadResult.Url;
                lessonRecord.DurationSeconds = durationSeconds;
                lessonRecord.RecordingStartedAt = lesson.MeetingStartedAt;
                lessonRecord.RecordingEndedAt = lesson.MeetingEndedAt;
                lessonRecordRepo.Update(lessonRecord);
            }

            await _unitOfWork.SaveChangesAsync();

            foreach (var chunk in orderedChunks)
            {
                _ = await _driveService.DeleteFileAsync(chunk.RelativePath, context.CancellationToken);
            }

            _logger.LogInformation("End ProcessMeetingRecordingAfterEndedEventConsumer for lesson {LessonId}", eventData.LessonId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error consuming ProcessMeetingRecordingAfterEndedEvent for lesson {LessonId}",
                eventData.LessonId);
            throw;
        }
        finally
        {
            try
            {
                if (Directory.Exists(tempWorkingFolder))
                {
                    Directory.Delete(tempWorkingFolder, true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to clean temp merge folder {TempWorkingFolder}", tempWorkingFolder);
            }
        }
    }

    private static int CalculateDurationSeconds(Lesson lesson, int chunkCount)
    {
        if (lesson.MeetingStartedAt.HasValue && lesson.MeetingEndedAt.HasValue)
        {
            return (int)Math.Max(0, (lesson.MeetingEndedAt.Value - lesson.MeetingStartedAt.Value).TotalSeconds);
        }

        return chunkCount * DefaultChunkSeconds;
    }

    private static int GetChunkOrder(string fileName)
    {
        var match = Regex.Match(fileName, @"chunk-(\d+)", RegexOptions.IgnoreCase);
        if (match.Success && int.TryParse(match.Groups[1].Value, out var order))
        {
            return order;
        }

        return int.MaxValue;
    }
}
