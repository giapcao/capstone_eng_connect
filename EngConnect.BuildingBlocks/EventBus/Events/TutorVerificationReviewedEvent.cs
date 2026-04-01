using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.EventBus.Abstraction;

namespace EngConnect.BuildingBlock.EventBus.Events;

public class TutorVerificationReviewedEvent : NotificationEvent
{
    public string TutorEmail { get; set; } = null!;
    public string TutorFullName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? RejectionReason { get; set; }

    public static TutorVerificationReviewedEvent Create(
        Guid adminUserId,
        Guid tutorId,
        string tutorEmail,
        string tutorFullName,
        string status,
        string? rejectionReason)
    {
        return new TutorVerificationReviewedEvent
        {
            IssuerId = adminUserId,
            ResourceId = tutorId.ToString(),
            ResourceType = nameof(EventResourceType.Tutor),
            TutorEmail = tutorEmail,
            TutorFullName = tutorFullName,
            Status = status,
            RejectionReason = rejectionReason
        };
    }
}
