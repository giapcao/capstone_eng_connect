using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class LessonRecordConfiguration : IEntityTypeConfiguration<LessonRecord>
{
    public void Configure(EntityTypeBuilder<LessonRecord> builder)
    {
        builder.HasKey(e => e.Id).HasName("lesson_record_pkey");

        builder.ToTable("lesson_record");

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.LessonId)
            .HasColumnName("lesson_id");

        builder.Property(e => e.RecordUrl)
            .HasColumnName("record_url");

        builder.Property(e => e.DurationSeconds)
            .HasColumnName("duration_seconds");

        builder.Property(e => e.RecordingStartedAt)
            .HasColumnName("recording_started_at");

        builder.Property(e => e.RecordingEndedAt)
            .HasColumnName("recording_ended_at");

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("now()");

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("now()");

        builder.Property(e => e.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);

        builder.Property(e => e.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasOne(d => d.Lesson)
            .WithMany(p => p.LessonRecords)
            .HasForeignKey(d => d.LessonId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_record_lesson");
    }
}

