using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class OutboxEventConfiguration : IEntityTypeConfiguration<OutboxEvent>
{
    public void Configure(EntityTypeBuilder<OutboxEvent> builder)
    {
        builder.HasKey(e => e.Id).HasName("outbox_event_pkey");

        builder.ToTable("outbox_event");

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.AggregateType)
            .HasMaxLength(50)
            .HasColumnName("aggregate_type")
            .IsRequired();

        builder.Property(e => e.AggregateId)
            .HasColumnName("aggregate_id");

        builder.Property(e => e.EventType)
            .HasMaxLength(100)
            .HasColumnName("event_type")
            .IsRequired();

        builder.Property(e => e.EventData)
            .HasColumnName("event_data")
            .HasConversion(
                v => v.ToString() ?? string.Empty,
                v => v)
            .IsRequired();

        builder.Property(e => e.OutboxStatus)
            .HasMaxLength(30)
            .HasColumnType("varchar(30)")
            .HasColumnName("outbox_status")
            .IsRequired();

        builder.Property(e => e.ProcessedAt)
            .HasColumnName("processed_at");

        builder.Property(e => e.SentAt)
            .HasColumnName("sent_at");

        builder.Property(e => e.FailedAt)
            .HasColumnName("failed_at");

        builder.Property(e => e.DeadAt)
            .HasColumnName("dead_at");

        builder.Property(e => e.LockBy)
            .HasMaxLength(100)
            .HasColumnName("lock_by");

        builder.Property(e => e.LockAt)
            .HasColumnName("lock_at");

        builder.Property(e => e.RetryCount)
            .HasColumnName("retry_count")
            .HasDefaultValue(0);

        builder.Property(e => e.NextRetryAt)
            .HasColumnName("next_retry_at");

        builder.Property(e => e.LastError)
            .HasColumnName("last_error");

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("now()");
    }
}

