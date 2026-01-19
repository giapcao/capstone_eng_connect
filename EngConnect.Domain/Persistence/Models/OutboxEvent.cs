using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class OutboxEvent : AuditableEntity<Guid>
{
    public string AggregateType { get; set; } = null!;

    public Guid AggregateId { get; set; }

    public string EventType { get; set; } = null!;

    public string EventData { get; set; } = null!;

    public string OutboxStatus { get; set; } = null!;

    public DateTime? ProcessedAt { get; set; }

    public DateTime? SentAt { get; set; }

    public DateTime? FailedAt { get; set; }

    public DateTime? DeadAt { get; set; }

    public string? LockBy { get; set; }

    public DateTime? LockAt { get; set; }

    public int? RetryCount { get; set; }

    public DateTime? NextRetryAt { get; set; }

    public string? LastError { get; set; }
}
