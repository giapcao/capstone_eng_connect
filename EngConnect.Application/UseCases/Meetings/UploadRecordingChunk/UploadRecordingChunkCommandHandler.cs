using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.BuildingBlock.EventBus.Events;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Meetings.UploadRecordingChunk;

public class UploadRecordingChunkCommandHandler : ICommandHandler<UploadRecordingChunkCommand>
{
    private const string TempRecordingRootFolder = "engconnect/meeting-recordings";
    private readonly ILogger<UploadRecordingChunkCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UploadRecordingChunkCommandHandler(
        ILogger<UploadRecordingChunkCommandHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UploadRecordingChunkCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UploadRecordingChunkCommandHandler: {@Command}", command);

        try
        {
            var lessonRepo = _unitOfWork.GetRepository<Lesson, Guid>();
            var lesson = await lessonRepo.FindByIdAsync(command.LessonId, cancellationToken: cancellationToken);

            if (lesson == null)
            {
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Lesson>("Lesson"));
            }

            var tutorRepo = _unitOfWork.GetRepository<Tutor, Guid>();
            var studentRepo = _unitOfWork.GetRepository<Student, Guid>();

            var isTutor = await tutorRepo.AnyAsync(
                x => x.Id == lesson.TutorId && x.UserId == command.UserId,
                cancellationToken: cancellationToken);

            var isStudent = await studentRepo.AnyAsync(
                x => x.Id == lesson.StudentId && x.UserId == command.UserId,
                cancellationToken: cancellationToken);

            if (!isTutor && !isStudent)
            {
                return Result.Failure(HttpStatusCode.Forbidden, CommonErrors.Unauthorized<Lesson>());
            }

            if (lesson.MeetingStatus == nameof(MeetingStatus.Ended))
            {
                return Result.Failure(HttpStatusCode.BadRequest,
                    CommonErrors.InvalidOperation("Cannot upload recording chunk after meeting ended."));
            }

            var extension = Path.GetExtension(command.File.FileName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                extension = ".webm";
            }

            var lessonTempFolder = Path.Combine(
                Path.GetTempPath(),
                TempRecordingRootFolder,
                $"lesson-{command.LessonId}");
            Directory.CreateDirectory(lessonTempFolder);

            var tempFilePath = Path.Combine(lessonTempFolder, $"chunk-{command.ChunkTimestamp}{extension}");

            if (command.File.Content.CanSeek)
            {
                command.File.Content.Position = 0;
            }

            await using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await command.File.Content.CopyToAsync(fileStream, cancellationToken);
            }

            var uploadChunkEvent = UploadMeetingRecordingChunkEvent.Create(
                command.LessonId,
                command.UserId,
                command.ChunkTimestamp,
                tempFilePath,
                Path.GetFileName(command.File.FileName),
                string.IsNullOrWhiteSpace(command.File.ContentType) ? "video/webm" : command.File.ContentType);

            var outboxEventRepo = _unitOfWork.GetRepository<OutboxEvent, Guid>();
            var outboxEvent = OutboxEvent.CreateOutboxEvent(nameof(Lesson), command.LessonId, uploadChunkEvent);
            outboxEventRepo.Add(outboxEvent);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UploadRecordingChunkCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UploadRecordingChunkCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
