using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Meetings.UploadRecordingChunk;

public class UploadRecordingChunkCommandHandler : ICommandHandler<UploadRecordingChunkCommand, FileUploadResult>
{
    private readonly ILogger<UploadRecordingChunkCommandHandler> _logger;
    private readonly IDriveService _driveService;
    private readonly IUnitOfWork _unitOfWork;

    public UploadRecordingChunkCommandHandler(
        ILogger<UploadRecordingChunkCommandHandler> logger,
        IDriveService driveService,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _driveService = driveService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<FileUploadResult>> HandleAsync(UploadRecordingChunkCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UploadRecordingChunkCommandHandler: {@Command}", command);

        try
        {
            var lessonRepo = _unitOfWork.GetRepository<Lesson, Guid>();
            var lesson = await lessonRepo.FindByIdAsync(command.LessonId, cancellationToken: cancellationToken);

            if (lesson == null)
            {
                return Result.Failure<FileUploadResult>(HttpStatusCode.NotFound, CommonErrors.NotFound<Lesson>("Lesson"));
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
                return Result.Failure<FileUploadResult>(HttpStatusCode.Forbidden, CommonErrors.Unauthorized<Lesson>());
            }

            if (lesson.MeetingStatus == nameof(MeetingStatus.Ended))
            {
                return Result.Failure<FileUploadResult>(HttpStatusCode.BadRequest,
                    CommonErrors.InvalidOperation("Cannot upload recording chunk after meeting ended."));
            }

            var uploadResult = await _driveService.UploadMeetingChunkAsync(
                command.LessonId,
                command.ChunkIndex,
                command.File,
                cancellationToken);

            _logger.LogInformation("End UploadRecordingChunkCommandHandler");
            return Result.Success(uploadResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UploadRecordingChunkCommandHandler: {Message}", ex.Message);
            return Result.Failure<FileUploadResult>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
