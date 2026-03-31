using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class LessonRecord : AuditableEntity<Guid>
{
    public Guid LessonId { get; set; }

    public string RecordUrl { get; set; } = null!;

    public int? DurationSeconds { get; set; }

    public DateTime? RecordingStartedAt { get; set; }

    public DateTime? RecordingEndedAt { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual LessonScript? LessonScript { get; set; }
}
