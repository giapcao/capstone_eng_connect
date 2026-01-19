using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class TutorSchedule : AuditableEntity<Guid>
{
    public Guid TutorId { get; set; }

    public string? Weekday { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public virtual Tutor Tutor { get; set; } = null!;
}
