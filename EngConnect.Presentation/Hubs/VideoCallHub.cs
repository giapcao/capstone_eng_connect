using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.Application.UseCases.Meetings.EndMeeting;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EngConnect.Presentation.Hubs;

[Authorize]
public class VideoCallHub : Hub
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<VideoCallHub> _logger;

    public VideoCallHub(IUnitOfWork unitOfWork, ICommandDispatcher commandDispatcher, ILogger<VideoCallHub> logger)
    {
        _unitOfWork = unitOfWork;
        _commandDispatcher = commandDispatcher;
        _logger = logger;
    }

    /// <summary>
    /// Tutor creates a video call room for a lesson
    /// </summary>
    public async Task CreateRoom(Guid lessonId)
    {
        var userId = GetUserId();
        _logger.LogInformation("CreateRoom called by UserId: {UserId} for LessonId: {LessonId}", userId, lessonId);

        try
        {
            var lessonRepo = _unitOfWork.GetRepository<Lesson, Guid>();
            var lesson = await lessonRepo.FindByIdAsync(lessonId);

            if (lesson == null)
            {
                await Clients.Caller.SendAsync("Error", "Không tìm thấy buổi học.");
                return;
            }

            // Verify the caller is the tutor of this lesson
            var tutorRepo = _unitOfWork.GetRepository<Tutor, Guid>();
            var tutor = await tutorRepo.FindFirstAsync(t => t.Id == lesson.TutorId && t.UserId == userId);
            if (tutor == null)
            {
                await Clients.Caller.SendAsync("Error", "Người dùng không có quyền tạo phòng học cho buổi học này.");
                return;
            }

            if (lesson.MeetingStatus == nameof(MeetingStatus.InProgress))
            {
                await Clients.Caller.SendAsync("Error", "Phòng học đã đang diễn ra.");
                return;
            }

            if (lesson.MeetingStatus == nameof(MeetingStatus.Ended))
            {
                await Clients.Caller.SendAsync("Error", "Buổi học đã kết thúc.");
                return;
            }

            // Update lesson meeting status
            var roomId = $"lesson-{lessonId}";
            lesson.MeetingStatus = nameof(MeetingStatus.Waiting);
            lesson.MeetingUrl = roomId;
            lessonRepo.Update(lesson);

            // Create participant record for tutor
            var participantRepo = _unitOfWork.GetRepository<MeetingParticipant, Guid>();
            var participant = new MeetingParticipant
            {
                LessonId = lessonId,
                UserId = userId,
                Role = "Tutor",
                JoinedAt = DateTime.UtcNow,
                ConnectionId = Context.ConnectionId
            };
            participantRepo.Add(participant);

            await _unitOfWork.SaveChangesAsync();

            // Add tutor to SignalR group
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            await Clients.Caller.SendAsync("RoomCreated", new
            {
                RoomId = roomId,
                LessonId = lessonId,
                MeetingStatus = lesson.MeetingStatus
            });

            _logger.LogInformation("Room {RoomId} created by tutor {UserId}", roomId, userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateRoom for LessonId: {LessonId}", lessonId);
            await Clients.Caller.SendAsync("Error", "Có lỗi xảy ra khi tạo phòng học.");
        }
    }

    /// <summary>
    /// Student joins an existing video call room
    /// </summary>
    public async Task JoinRoom(Guid lessonId)
    {
        var userId = GetUserId();
        _logger.LogInformation("JoinRoom called by UserId: {UserId} for LessonId: {LessonId}", userId, lessonId);

        try
        {
            var lessonRepo = _unitOfWork.GetRepository<Lesson, Guid>();
            var lesson = await lessonRepo.FindByIdAsync(lessonId);

            if (lesson == null)
            {
                await Clients.Caller.SendAsync("Error", "Không tìm thấy buổi học.");
                return;
            }

            // Verify the caller
            var tutorRepo = _unitOfWork.GetRepository<Tutor, Guid>();
            var studentRepo = _unitOfWork.GetRepository<Student, Guid>();
            var tutor = await tutorRepo.FindFirstAsync(t => t.Id == lesson.TutorId && t.UserId == userId);
            var student = await studentRepo.FindFirstAsync(s => s.Id == lesson.StudentId && s.UserId == userId);

            if (tutor == null && student == null)
            {
                await Clients.Caller.SendAsync("Error", "Người dùng không có quyền tham gia buổi học này.");
                return;
            }

            if (lesson.MeetingStatus == nameof(MeetingStatus.Ended))
            {
                await Clients.Caller.SendAsync("Error", "Buổi học đã kết thúc.");
                return;
            }

            if (string.IsNullOrEmpty(lesson.MeetingUrl))
            {
                await Clients.Caller.SendAsync("Error", "Phòng học chưa được tạo.");
                return;
            }

            var roomId = lesson.MeetingUrl;
            var role = tutor != null ? "Tutor" : "Student";

            // Check if user already has an active participant record
            var participantRepo = _unitOfWork.GetRepository<MeetingParticipant, Guid>();
            var existingParticipant = await participantRepo.FindFirstAsync(
                p => p.LessonId == lessonId && p.UserId == userId && p.LeftAt == null);

            if (existingParticipant != null)
            {
                // Update connection ID for reconnection scenario
                existingParticipant.ConnectionId = Context.ConnectionId;
                participantRepo.Update(existingParticipant);
            }
            else
            {
                // Create new participant record
                var participant = new MeetingParticipant
                {
                    LessonId = lessonId,
                    UserId = userId,
                    Role = role,
                    JoinedAt = DateTime.UtcNow,
                    ConnectionId = Context.ConnectionId
                };
                participantRepo.Add(participant);
            }

            // If both tutor and student are now in the room, mark as InProgress
            if (lesson.MeetingStatus == nameof(MeetingStatus.Waiting))
            {
                lesson.MeetingStatus = nameof(MeetingStatus.InProgress);
                lesson.MeetingStartedAt = DateTime.UtcNow;
                lessonRepo.Update(lesson);
            }

            await _unitOfWork.SaveChangesAsync();

            // Add to SignalR group
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

            // Notify the caller about successful join
            await Clients.Caller.SendAsync("RoomJoined", new
            {
                RoomId = roomId,
                LessonId = lessonId,
                UserId = userId,
                Role = role,
                MeetingStatus = lesson.MeetingStatus
            });

            // Notify other participants in the room that a new user joined
            await Clients.OthersInGroup(roomId).SendAsync("UserJoined", new
            {
                UserId = userId,
                Role = role,
                ConnectionId = Context.ConnectionId
            });

            _logger.LogInformation("User {UserId} ({Role}) joined room {RoomId}", userId, role, roomId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in JoinRoom for LessonId: {LessonId}", lessonId);
            await Clients.Caller.SendAsync("Error", "Có lỗi xảy ra khi tham gia phòng học.");
        }
    }

    /// <summary>
    /// Relay WebRTC SDP offer to the target user
    /// </summary>
    public async Task SendOffer(Guid lessonId, string targetConnectionId, object sdp)
    {
        var userId = GetUserId();

        _logger.LogInformation("SendOffer from {UserId} to {TargetConnectionId}", userId, targetConnectionId);

        await Clients.Client(targetConnectionId).SendAsync("ReceiveOffer", new
        {
            CallerConnectionId = Context.ConnectionId,
            UserId = userId,
            Sdp = sdp
        });
    }

    /// <summary>
    /// Relay WebRTC SDP answer to the target user
    /// </summary>
    public async Task SendAnswer(Guid lessonId, string targetConnectionId, object sdp)
    {
        var userId = GetUserId();

        _logger.LogInformation("SendAnswer from {UserId} to {TargetConnectionId}", userId, targetConnectionId);

        await Clients.Client(targetConnectionId).SendAsync("ReceiveAnswer", new
        {
            CalleeConnectionId = Context.ConnectionId,
            UserId = userId,
            Sdp = sdp
        });
    }

    /// <summary>
    /// Relay ICE candidate to the target user
    /// </summary>
    public async Task SendIceCandidate(Guid lessonId, string targetConnectionId, object candidate)
    {
        var userId = GetUserId();

        await Clients.Client(targetConnectionId).SendAsync("ReceiveIceCandidate", new
        {
            ConnectionId = Context.ConnectionId,
            UserId = userId,
            Candidate = candidate
        });
    }

    /// <summary>
    /// User leaves the video call room
    /// </summary>
    public async Task LeaveRoom(Guid lessonId)
    {
        var userId = GetUserId();
        _logger.LogInformation("LeaveRoom called by UserId: {UserId} for LessonId: {LessonId}", userId, lessonId);

        try
        {
            var lessonRepo = _unitOfWork.GetRepository<Lesson, Guid>();
            var lesson = await lessonRepo.FindByIdAsync(lessonId);

            if (lesson == null) return;

            var roomId = lesson.MeetingUrl;
            if (string.IsNullOrEmpty(roomId)) return;

            // Update participant leave time
            var participantRepo = _unitOfWork.GetRepository<MeetingParticipant, Guid>();
            var participant = await participantRepo.FindFirstAsync(
                p => p.LessonId == lessonId && p.UserId == userId && p.LeftAt == null);

            if (participant != null)
            {
                participant.LeftAt = DateTime.UtcNow;
                participant.ConnectionId = null;
                participantRepo.Update(participant);
            }

            await _unitOfWork.SaveChangesAsync();

            // Remove from SignalR group
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

            // Notify others
            await Clients.OthersInGroup(roomId).SendAsync("UserLeft", new
            {
                UserId = userId,
                ConnectionId = Context.ConnectionId
            });

            _logger.LogInformation("User {UserId} left room {RoomId}", userId, roomId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in LeaveRoom for LessonId: {LessonId}", lessonId);
        }
    }

    /// <summary>
    /// Tutor ends the meeting for everyone
    /// </summary>
    public async Task EndMeeting(Guid lessonId)
    {
        var userId = GetUserId();
        _logger.LogInformation("EndMeeting called by UserId: {UserId} for LessonId: {LessonId}", userId, lessonId);

        try
        {
            var lessonRepo = _unitOfWork.GetRepository<Lesson, Guid>();
            var lesson = await lessonRepo.FindByIdAsync(lessonId);

            if (lesson == null)
            {
                await Clients.Caller.SendAsync("Error", "Không tìm thấy buổi học.");
                return;
            }

            var endMeetingResult = await _commandDispatcher.DispatchAsync(
                new EndMeetingCommand(lessonId, userId));

            if (endMeetingResult.IsFailure)
            {
                await Clients.Caller.SendAsync("Error", endMeetingResult.Error?.Message ?? "Không thể kết thúc buổi học.");
                return;
            }

            var roomId = lesson.MeetingUrl;
            if (string.IsNullOrEmpty(roomId)) return;

            lesson = await lessonRepo.FindByIdAsync(lessonId);

            // Notify everyone in the room
            await Clients.Group(roomId).SendAsync("MeetingEnded", new
            {
                LessonId = lessonId,
                EndedAt = lesson?.MeetingEndedAt,
                EndedBy = userId
            });

            _logger.LogInformation("Meeting ended for room {RoomId} by tutor {UserId}", roomId, userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in EndMeeting for LessonId: {LessonId}", lessonId);
            await Clients.Caller.SendAsync("Error", "Có lỗi xảy ra khi kết thúc buổi học.");
        }
    }

    /// <summary>
    /// Toggle media (audio/video) and notify others
    /// </summary>
    public async Task ToggleMedia(Guid lessonId, bool isAudioEnabled, bool isVideoEnabled)
    {
        var userId = GetUserId();
        var roomId = $"lesson-{lessonId}";

        await Clients.OthersInGroup(roomId).SendAsync("MediaToggled", new
        {
            UserId = userId,
            ConnectionId = Context.ConnectionId,
            IsAudioEnabled = isAudioEnabled,
            IsVideoEnabled = isVideoEnabled
        });
    }

    /// <summary>
    /// Handle disconnection - update participant records
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("User disconnected: {ConnectionId}", Context.ConnectionId);

        try
        {
            var participantRepo = _unitOfWork.GetRepository<MeetingParticipant, Guid>();
            var activeParticipants = participantRepo.FindAll(
                p => p.ConnectionId == Context.ConnectionId && p.LeftAt == null, tracking: true);

            foreach (var participant in activeParticipants)
            {
                participant.LeftAt = DateTime.UtcNow;
                participant.ConnectionId = null;
                participantRepo.Update(participant);

                var lessonRepo = _unitOfWork.GetRepository<Lesson, Guid>();
                var lesson = await lessonRepo.FindByIdAsync(participant.LessonId);

                if (lesson?.MeetingUrl != null)
                {
                    await Clients.Group(lesson.MeetingUrl).SendAsync("UserLeft", new
                    {
                        participant.UserId,
                        ConnectionId = Context.ConnectionId
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling disconnection for {ConnectionId}", Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    private Guid GetUserId()
    {
        var userIdClaim = Context.User?.FindFirst("sub")?.Value
                          ?? Context.User?.FindFirst("userId")?.Value;
        return Guid.Parse(userIdClaim!);
    }
}