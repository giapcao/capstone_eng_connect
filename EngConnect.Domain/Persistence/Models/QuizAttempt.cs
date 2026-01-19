using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class QuizAttempt : AuditableEntity<Guid>
{
    public Guid QuizId { get; set; }

    public Guid StudentId { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public decimal? Score { get; set; }

    public int? DurationSeconds { get; set; }

    public string? Status { get; set; }
}
