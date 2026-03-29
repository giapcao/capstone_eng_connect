using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class MeetingParticipant : AuditableEntity<Guid>
{
    public Guid LessonId { get; set; }

    public Guid UserId { get; set; }

    public string Role { get; set; } = null!; // Tutor or Student

    public DateTime? JoinedAt { get; set; }

    public DateTime? LeftAt { get; set; }

    public string? ConnectionId { get; set; } // SignalR connection ID

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}