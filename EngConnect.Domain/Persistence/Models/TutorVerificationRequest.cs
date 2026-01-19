using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class TutorVerificationRequest : AuditableEntity<Guid>
{
    public Guid TutorId { get; set; }

    public string? Status { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public Guid? ReviewedBy { get; set; }

    public string? RejectionReason { get; set; }

    public virtual User? ReviewedByNavigation { get; set; }

    public virtual Tutor Tutor { get; set; } = null!;
}
