using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.BuildingBlock.EventBus.Constants;
using EngConnect.BuildingBlock.EventBus.Events;
using EngConnect.BuildingBlock.EventBus.Utils;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Meetings.EndMeeting;

public class EndMeetingCommandHandler : ICommandHandler<EndMeetingCommand>
{
    private readonly ILogger<EndMeetingCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public EndMeetingCommandHandler(
        ILogger<EndMeetingCommandHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
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

            var meetingEndedEvent = ProcessMeetingRecordingAfterEndedEvent.Create(command.LessonId, command.UserId, command.TotalChunks);
            var outboxEventRepo = _unitOfWork.GetRepository<OutboxEvent, Guid>();
            var notificationEvent = NotificationHelper.CreateNotification(meetingEndedEvent, 
                [],[],nameof(Channel.System));
            var outboxEvent = OutboxEvent.CreateOutboxEvent(nameof(Lesson), command.LessonId, notificationEvent);
            outboxEventRepo.Add(outboxEvent);

            await _unitOfWork.SaveChangesAsync();

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
}