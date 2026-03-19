using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(e => e.Id).HasName("lesson_pkey");

        builder.ToTable("lesson");

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.TutorId)
            .HasColumnName("tutor_id");

        builder.Property(e => e.StudentId)
            .HasColumnName("student_id");

        builder.Property(e => e.EnrollmentId)
            .HasColumnName("enrollment_id");

        builder.Property(e => e.SessionId)
            .HasColumnName("session_id");

        builder.Property(e => e.StartTime)
            .HasColumnName("start_time");

        builder.Property(e => e.EndTime)
            .HasColumnName("end_time");

        builder.Property(e => e.Status)
            .HasMaxLength(30)
            .HasColumnName("status");

        builder.Property(e => e.MeetingUrl)
            .HasColumnName("meeting_url");

        builder.Property(e => e.MeetingStatus)
            .HasMaxLength(30)
            .HasColumnName("meeting_status");

        builder.Property(e => e.MeetingStartedAt)
            .HasColumnName("meeting_started_at");

        builder.Property(e => e.MeetingEndedAt)
            .HasColumnName("meeting_ended_at");

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

        builder.HasOne(d => d.Enrollment)
            .WithMany(p => p.Lessons)
            .HasForeignKey(d => d.EnrollmentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_lesson_enrollment");

        builder.HasOne(d => d.Session)
            .WithMany(p => p.Lessons)
            .HasForeignKey(d => d.SessionId)
            .HasConstraintName("fk_lesson_session");

        builder.HasOne(d => d.Student)
            .WithMany(p => p.Lessons)
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_lesson_student");

        builder.HasOne(d => d.Tutor)
            .WithMany(p => p.Lessons)
            .HasForeignKey(d => d.TutorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_lesson_tutor");
    }
}

