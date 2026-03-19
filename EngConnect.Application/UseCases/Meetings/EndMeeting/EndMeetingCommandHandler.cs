using System.Net;
using System.Text.RegularExpressions;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Meetings.EndMeeting;

public class EndMeetingCommandHandler : ICommandHandler<EndMeetingCommand>
{
    private const int DefaultChunkSeconds = 30;
    private const string LessonRecordPrefix = "lesson-records";
    private readonly ILogger<EndMeetingCommandHandler> _logger;
    private readonly IAwsStorageService _awsStorageService;
    private readonly IDriveService _driveService;
    private readonly IUnitOfWork _unitOfWork;

    public EndMeetingCommandHandler(
        ILogger<EndMeetingCommandHandler> logger,
        IAwsStorageService awsStorageService,
        IDriveService driveService,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _awsStorageService = awsStorageService;
        _driveService = driveService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(
        EndMeetingCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start EndMeetingCommandHandler: {@Command}", command);

        try
        {
            var lessonRepo = _unitOfWork.GetRepository<Lesson, Guid>();
            var lesson = await lessonRepo.FindByIdAsync(command.LessonId, cancellationToken: cancellationToken);

            if (lesson == null)
            {
                return Result.Failure(HttpStatusCode.NotFound,
                    MeetingErrors.LessonNotFound(command.LessonId));
            }

            // Verify the caller is the tutor
            var tutorRepo = _unitOfWork.GetRepository<Tutor, Guid>();
            var tutor = await tutorRepo.FindFirstAsync(
                t => t.Id == lesson.TutorId && t.UserId == command.UserId,
                cancellationToken: cancellationToken);

            if (tutor == null)
            {
                return Result.Failure(HttpStatusCode.Forbidden,
                    MeetingErrors.NotAuthorized());
            }

            if (lesson.MeetingStatus == nameof(MeetingStatus.Ended))
            {
                return Result.Failure(HttpStatusCode.BadRequest,
                    MeetingErrors.MeetingAlreadyEnded(command.LessonId));
            }

            // End the meeting
            lesson.MeetingStatus = nameof(MeetingStatus.Ended);
            lesson.MeetingEndedAt = DateTime.UtcNow;
            lessonRepo.Update(lesson);

            // Close all active participants
            var participantRepo = _unitOfWork.GetRepository<MeetingParticipant, Guid>();
            var activeParticipants = participantRepo.FindAll(
                p => p.LessonId == command.LessonId && p.LeftAt == null, tracking: true, cancellationToken);

            foreach (var participant in activeParticipants)
            {
                participant.LeftAt = DateTime.UtcNow;
                participant.ConnectionId = null;
                participantRepo.Update(participant);
            }

            await _unitOfWork.SaveChangesAsync();

            await BuildAndSaveLessonRecordAsync(lesson, command.UserId, cancellationToken);

            _logger.LogInformation("End EndMeetingCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in EndMeetingCommandHandler {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }

    private async Task BuildAndSaveLessonRecordAsync(Lesson lesson, Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var chunks = await _driveService.GetMeetingChunksAsync(lesson.Id, cancellationToken);
            if (chunks.Count == 0)
            {
                _logger.LogInformation("No recording chunks found for lesson {LessonId}", lesson.Id);
                return;
            }

            var orderedChunks = chunks
                .OrderBy(x => GetChunkOrder(x.StoredFileName))
                .ThenBy(x => x.StoredFileName)
                .ToList();

            await using var mergedStream = new MemoryStream();
            foreach (var chunk in orderedChunks)
            {
                await using var chunkStream = await _driveService.DownloadFileAsync(chunk.RelativePath, cancellationToken);
                await chunkStream.CopyToAsync(mergedStream, cancellationToken);
            }

            mergedStream.Position = 0;

            var firstChunk = orderedChunks[0];
            var extension = Path.GetExtension(firstChunk.StoredFileName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".webm";
            }

            var mergedFile = new FileUpload
            {
                FileName = $"lesson-{lesson.Id}-{DateTime.UtcNow:yyyyMMddHHmmss}{extension}",
                ContentType = string.IsNullOrWhiteSpace(firstChunk.ContentType) ? "video/webm" : firstChunk.ContentType,
                Length = mergedStream.Length,
                Content = mergedStream
            };

            var awsUploadResult = await _awsStorageService.UploadFileAsync(
                mergedFile,
                userId,
                LessonRecordPrefix,
                cancellationToken);

            var lessonRecordRepo = _unitOfWork.GetRepository<LessonRecord, Guid>();
            var lessonRecord = await lessonRecordRepo.FindFirstAsync(
                x => x.LessonId == lesson.Id,
                tracking: true,
                cancellationToken: cancellationToken);

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
                _ = await _driveService.DeleteFileAsync(chunk.RelativePath, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to build merged recording for lesson {LessonId} after meeting end.",
                lesson.Id);
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