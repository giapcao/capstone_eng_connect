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
    private const string TempRecordingRootFolder = "engconnect/meeting-recordings";
    private readonly IAwsStorageService _awsStorageService;
    private readonly ILogger<ProcessMeetingRecordingAfterEndedEventConsumer> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ProcessMeetingRecordingAfterEndedEventConsumer(
        IAwsStorageService awsStorageService,
        IUnitOfWork unitOfWork,
        ILogger<ProcessMeetingRecordingAfterEndedEventConsumer> logger)
    {
        _awsStorageService = awsStorageService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ProcessMeetingRecordingAfterEndedEvent> context)
    {
        var eventData = context.Message;
        _logger.LogInformation("Start ProcessMeetingRecordingAfterEndedEventConsumer {@EventData}", eventData);

        var lessonTempFolder = Path.Combine(
            Path.GetTempPath(),
            TempRecordingRootFolder,
            $"lesson-{eventData.LessonId}");

        var tempWorkingFolder = Path.Combine(
            lessonTempFolder,
            $"merge-{Guid.NewGuid():N}");

        try
        {
            if (!Directory.Exists(lessonTempFolder))
            {
                _logger.LogInformation("Temp lesson folder not found for lesson {LessonId}", eventData.LessonId);
                return;
            }

            var chunks = Directory.GetFiles(lessonTempFolder, "chunk-*.*", SearchOption.TopDirectoryOnly)
                .ToList();

            if (chunks.Count == 0)
            {
                _logger.LogInformation("No temp recording chunks found for lesson {LessonId}", eventData.LessonId);
                return;
            }

            var orderedChunks = chunks
                .OrderBy(GetChunkTimestamp)
                .ThenBy(Path.GetFileName)
                .ToList();

            var firstChunkPath = orderedChunks[0];
            var extension = Path.GetExtension(firstChunkPath);
            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".webm";
            }

            Directory.CreateDirectory(tempWorkingFolder);
            var mergedFileName = $"lesson-{eventData.LessonId}-{DateTime.UtcNow:yyyyMMddHHmmss}{extension}";
            var mergedFilePath = Path.Combine(tempWorkingFolder, mergedFileName);

            await using (var mergedOutputStream = new FileStream(mergedFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                foreach (var chunkPath in orderedChunks)
                {
                    await using var chunkStream = new FileStream(chunkPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    await chunkStream.CopyToAsync(mergedOutputStream, context.CancellationToken);
                }
            }

            var mergedFileInfo = new FileInfo(mergedFilePath);
            await using var uploadStream = new FileStream(mergedFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            var mergedFile = new FileUpload
            {
                FileName = mergedFileName,
                ContentType = "video/webm",
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

    private static long GetChunkTimestamp(string filePath)
    {
        var fileName = Path.GetFileName(filePath);
        var match = Regex.Match(fileName, @"chunk-(\d+)", RegexOptions.IgnoreCase);
        if (match.Success && long.TryParse(match.Groups[1].Value, out var timestamp))
        {
            return timestamp;
        }

        return long.MaxValue;
    }
}
