using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class TutorDocument : AuditableEntity<Guid>
{
    public Guid TutorId { get; set; }

    public string? Name { get; set; }

    public string? DocType { get; set; }

    public string Url { get; set; } = null!;

    public string? IssuedBy { get; set; }

    public DateOnly? IssuedAt { get; set; }

    public DateOnly? ExpiredAt { get; set; }

    public string? Status { get; set; }

    public virtual Tutor Tutor { get; set; } = null!;
}
