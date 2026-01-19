using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class CourseVerificationRequest : AuditableEntity<Guid>
{
    public Guid CourseId { get; set; }

    public string? Status { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public Guid? ReviewedBy { get; set; }

    public string? RejectionReason { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User? ReviewedByNavigation { get; set; }
}
