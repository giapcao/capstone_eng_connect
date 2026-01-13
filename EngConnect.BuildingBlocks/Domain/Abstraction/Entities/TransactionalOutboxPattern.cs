using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;
using EngConnect.BuildingBlock.Domain.Constants;

namespace EngConnect.BuildingBlock.Domain.Abstraction.Entities;

public abstract class TransactionalOutboxPattern : BaseEntity<Guid>
{
    // Aggregate info
    public string AggregateType { get; protected set; } = null!;
    public Guid AggregateId { get; protected set; }
    public string EventType { get; protected set; } = null!; // Ex: PaymentSucceededEvent, PaymentFailedEvent, ...
    public object EventData { get; protected set; } = null!; // Event data in json

    // Status tracking
    public OutboxStatus OutboxStatus { get; protected set; } = OutboxStatus.Pending;
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? ProcessedAt { get; protected set; }
    public DateTime? SentAt { get; protected set; }
    public DateTime? FailedAt { get; protected set; }
    public DateTime? DeadAt { get; protected set; }

    // Concurrency-safe lock
    public Guid? LockBy { get; protected set; }
    public DateTime? LockAt { get; protected set; }

    // Retry mechanism
    public int RetryCount { get; protected set; } = 0;
    public DateTime? NextRetryAt { get; protected set; }
    public string? LastError { get; protected set; }
}