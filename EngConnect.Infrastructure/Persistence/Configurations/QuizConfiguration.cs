using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasKey(e => e.Id).HasName("quiz_pkey");

        builder.ToTable("quiz");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(e => e.CourseId)
            .HasColumnName("course_id");

        builder.Property(e => e.Title)
            .HasMaxLength(255)
            .HasColumnName("title");

        builder.Property(e => e.Description)
            .HasColumnName("description");

        builder.Property(e => e.IsOpen)
            .HasColumnName("is_open")
            .HasDefaultValue(false);

        builder.Property(e => e.MaxScore)
            .HasColumnName("max_score");

        builder.Property(e => e.DurationSeconds)
            .HasColumnName("duration_seconds");

        builder.Property(e => e.AttemptLimit)
            .HasColumnName("attempt_limit")
            .HasDefaultValue(1);

        builder.Property(e => e.ExpiredAt)
            .HasColumnName("expired_at");

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

        builder.HasOne(d => d.Course)
            .WithMany(p => p.Quizzes)
            .HasForeignKey(d => d.CourseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_quiz_course");
    }
}

