using EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.EventBus.Abstraction;

namespace EngConnect.BuildingBlock.EventBus.Events;

public class WarningInvalidLessonEvent : NotificationEvent
{
    public string StudentName { get; set; } = null!;
    public string StudentEmail { get; set; } = null!;
    public string TutorName { get; set; } = null!;
    public string TutorEmail { get; set; } = null!;
    public decimal QualityPercentage { get; set; }
    public DetailResult Reason { get; set; } = null!;
    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public static WarningInvalidLessonEvent Create(
        Guid studentId, 
        string studentName, 
        string studentEmail,
        string tutorName, 
        string tutorEmail,
        DateTime? startTime,
        DateTime? endTime,
        decimal percentage, 
        DetailResult reason)
    {
        return new WarningInvalidLessonEvent()
        {
            IssuerId = studentId,
            ResourceId = studentName,
            ResourceType = nameof(EventResourceType.Lesson),
            StudentName = studentName,
            StudentEmail = studentEmail,
            TutorName = tutorName,
            TutorEmail = tutorEmail,
            StartTime = startTime,
            EndTime = endTime,
            QualityPercentage = percentage,
            Reason = reason
        };
    }
}