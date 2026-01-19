using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class LessonHomeworkConfiguration : IEntityTypeConfiguration<LessonHomework>
{
    public void Configure(EntityTypeBuilder<LessonHomework> builder)
    {
        builder.HasKey(e => e.Id).HasName("lesson_homework_pkey");

        builder.ToTable("lesson_homework");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.Property(e => e.LessonId)
            .HasColumnName("lesson_id");

        builder.Property(e => e.Title)
            .HasMaxLength(255)
            .HasColumnName("title");

        builder.Property(e => e.Description)
            .HasColumnName("description");

        builder.Property(e => e.ResourceUrl)
            .HasColumnName("resource_url");

        builder.Property(e => e.SubmissionUrl)
            .HasColumnName("submission_url");

        builder.Property(e => e.Score)
            .HasPrecision(5, 2)
            .HasColumnName("score");

        builder.Property(e => e.MaxScore)
            .HasPrecision(5, 2)
            .HasColumnName("max_score");

        builder.Property(e => e.TutorFeedback)
            .HasColumnName("tutor_feedback");

        builder.Property(e => e.AssignedAt)
            .HasColumnName("assigned_at");

        builder.Property(e => e.SubmittedAt)
            .HasColumnName("submitted_at");

        builder.Property(e => e.DueAt)
            .HasColumnName("due_at");

        builder.Property(e => e.Status)
            .HasMaxLength(30)
            .HasColumnName("status")
            .HasDefaultValueSql("'not_started'::character varying");

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
            .WithMany(p => p.LessonHomeworks)
            .HasForeignKey(d => d.LessonId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_hw_lesson");
    }
}

