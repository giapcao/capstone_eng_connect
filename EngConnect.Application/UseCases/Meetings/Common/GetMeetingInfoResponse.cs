namespace EngConnect.Application.UseCases.Meetings.GetMeetingInfo;

public record GetMeetingInfoResponse
{
    public Guid LessonId { get; init; }
    public string? RoomId { get; init; }
    public string? MeetingStatus { get; init; }
    public DateTime? MeetingStartedAt { get; init; }
    public DateTime? MeetingEndedAt { get; init; }
    public List<ParticipantInfo> Participants { get; init; } = [];
}

public record ParticipantInfo
{
    public Guid UserId { get; init; }
    public string Role { get; init; } = null!;
    public DateTime? JoinedAt { get; init; }
    public DateTime? LeftAt { get; init; }
}