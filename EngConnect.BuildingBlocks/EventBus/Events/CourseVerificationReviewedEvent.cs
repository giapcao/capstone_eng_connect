using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.EventBus.Abstraction;

namespace EngConnect.BuildingBlock.EventBus.Events;

public class CourseVerificationReviewedEvent : NotificationEvent
{
    public string TutorEmail { get; set; } = null!;
    public string TutorFullName { get; set; } = null!;
    public string CourseTitle { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? RejectionReason { get; set; }

    public static CourseVerificationReviewedEvent Create(
        Guid adminUserId,
        Guid courseId,
        string tutorEmail,
        string tutorFullName,
        string courseTitle,
        string status,
        string? rejectionReason)
    {
        return new CourseVerificationReviewedEvent
        {
            IssuerId = adminUserId,
            ResourceId = courseId.ToString(),
            ResourceType = nameof(EventResourceType.Course),
            TutorEmail = tutorEmail,
            TutorFullName = tutorFullName,
            CourseTitle = courseTitle,
            Status = status,
            RejectionReason = rejectionReason
        };
    }
}
