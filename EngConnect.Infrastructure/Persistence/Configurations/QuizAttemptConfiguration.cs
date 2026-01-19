using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
{
    public void Configure(EntityTypeBuilder<QuizAttempt> builder)
    {
        builder.HasKey(e => e.Id).HasName("quiz_attempt_pkey");

        builder.ToTable("quiz_attempt");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(e => e.QuizId)
            .HasColumnName("quiz_id");

        builder.Property(e => e.StudentId)
            .HasColumnName("student_id");

        builder.Property(e => e.StartedAt)
            .HasColumnName("started_at");

        builder.Property(e => e.CompletedAt)
            .HasColumnName("completed_at");

        builder.Property(e => e.Score)
            .HasPrecision(5, 2)
            .HasColumnName("score");

        builder.Property(e => e.DurationSeconds)
            .HasColumnName("duration_seconds");

        builder.Property(e => e.Status)
            .HasMaxLength(20)
            .HasColumnName("status");

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
    }
}

