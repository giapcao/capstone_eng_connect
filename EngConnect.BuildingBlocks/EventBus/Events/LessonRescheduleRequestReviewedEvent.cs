using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.EventBus.Abstraction;

namespace EngConnect.BuildingBlock.EventBus.Events;

public class LessonRescheduleRequestReviewedEvent : NotificationEvent
{
    public string StudentEmail { get; set; } = null!;
    public string StudentFullName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime ProposedStartTime { get; set; }
    public DateTime ProposedEndTime { get; set; }
    public string? TutorNote { get; set; }

    public static LessonRescheduleRequestReviewedEvent Create(
        Guid issuerId,
        Guid requestId,
        string studentEmail,
        string studentFullName,
        string status,
        DateTime proposedStartTime,
        DateTime proposedEndTime,
        string? tutorNote)
    {
        return new LessonRescheduleRequestReviewedEvent
        {
            IssuerId = issuerId,
            ResourceId = requestId.ToString(),
            ResourceType = nameof(EventResourceType.User),
            StudentEmail = studentEmail,
            StudentFullName = studentFullName,
            Status = status,
            ProposedStartTime = proposedStartTime,
            ProposedEndTime = proposedEndTime,
            TutorNote = tutorNote
        };
    }
}
