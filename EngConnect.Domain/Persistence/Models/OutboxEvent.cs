using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("outbox_event")]
public partial class OutboxEvent
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("aggregate_type")]
    [StringLength(50)]
    public string AggregateType { get; set; } = null!;

    [Column("aggregate_id")]
    public Guid AggregateId { get; set; }

    [Column("event_type")]
    [StringLength(100)]
    public string EventType { get; set; } = null!;

    [Column("event_data")]
    public string EventData { get; set; } = null!;

    [Column("outbox_status")]
    [StringLength(30)]
    public string OutboxStatus { get; set; } = null!;

    [Column("processed_at")]
    public DateTime? ProcessedAt { get; set; }

    [Column("sent_at")]
    public DateTime? SentAt { get; set; }

    [Column("failed_at")]
    public DateTime? FailedAt { get; set; }

    [Column("dead_at")]
    public DateTime? DeadAt { get; set; }

    [Column("lock_by")]
    [StringLength(100)]
    public string? LockBy { get; set; }

    [Column("lock_at")]
    public DateTime? LockAt { get; set; }

    [Column("retry_count")]
    public int? RetryCount { get; set; }

    [Column("next_retry_at")]
    public DateTime? NextRetryAt { get; set; }

    [Column("last_error")]
    public string? LastError { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}
