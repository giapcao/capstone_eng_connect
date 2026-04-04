using System.Text.RegularExpressions;
using System.Diagnostics;
using EngConnect.BuildingBlock.Contracts.Abstraction;
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

            var concatListFilePath = Path.Combine(tempWorkingFolder, "concat-list.txt");
            var concatLines = orderedChunks
                .Select(chunkPath => $"file '{chunkPath.Replace("'", "'\\''")}'")
                .ToArray();
            await File.WriteAllLinesAsync(concatListFilePath, concatLines, context.CancellationToken);

            await MergeChunksWithFfmpegAsync(concatListFilePath, mergedFilePath, context.CancellationToken);
            
            var awsUploadResult = await _awsStorageService.UploadFileFromPathAsync(
                mergedFilePath,
                eventData.EndedByUserId,
                LessonRecordPrefix,
                extension
                );

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
                if (Directory.Exists(lessonTempFolder))
                    Directory.Delete(lessonTempFolder, true);
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

    private async Task MergeChunksWithFfmpegAsync(string concatListFilePath, string outputFilePath,
        CancellationToken cancellationToken)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            Arguments = $"-y -f concat -safe 0 -i \"{concatListFilePath}\" -c copy \"{outputFilePath}\"",
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = startInfo };

        if (!process.Start())
        {
            throw new InvalidOperationException("Failed to start ffmpeg process.");
        }

        var stdOutTask = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var stdErrTask = process.StandardError.ReadToEndAsync(cancellationToken);

        await process.WaitForExitAsync(cancellationToken);

        var stdOut = await stdOutTask;
        var stdErr = await stdErrTask;

        if (process.ExitCode != 0)
        {
            _logger.LogError("ffmpeg failed with code {ExitCode}. StdOut: {StdOut}. StdErr: {StdErr}",
                process.ExitCode,
                stdOut,
                stdErr);
            throw new InvalidOperationException($"ffmpeg failed with exit code {process.ExitCode}.");
        }
    }

    private static string GetContentTypeFromExtension(string extension)
    {
        return extension.ToLowerInvariant() switch
        {
            ".mp4" => "video/mp4",
            ".webm" => "video/webm",
            ".mov" => "video/quicktime",
            ".mkv" => "video/x-matroska",
            _ => "application/octet-stream"
        };
    }
}
