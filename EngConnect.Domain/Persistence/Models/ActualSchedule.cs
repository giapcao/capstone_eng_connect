using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class ActualSchedule : AuditableEntity<Guid>
{
    public Guid TutorId { get; set; }

    public Guid? StudentId { get; set; }

    public Guid? CourseId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? Status { get; set; }

    public virtual Course? Course { get; set; }

    public virtual Student? Student { get; set; }

    public virtual Tutor Tutor { get; set; } = null!;
}