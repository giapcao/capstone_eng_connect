using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class ActualScheduleConfiguration : IEntityTypeConfiguration<ActualSchedule>
{
    public void Configure(EntityTypeBuilder<ActualSchedule> builder)
    {
        builder.HasKey(e => e.Id).HasName("actual_schedule_pkey");

        builder.ToTable("actual_schedule");

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.TutorId)
            .HasColumnName("tutor_id");

        builder.Property(e => e.StudentId)
            .HasColumnName("student_id");

        builder.Property(e => e.CourseId)
            .HasColumnName("course_id");

        builder.Property(e => e.StartTime)
            .HasColumnName("start_time");

        builder.Property(e => e.EndTime)
            .HasColumnName("end_time");

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

        builder.HasOne(d => d.Course)
            .WithMany(p => p.ActualSchedules)
            .HasForeignKey(d => d.CourseId)
            .HasConstraintName("fk_actual_course");

        builder.HasOne(d => d.Student)
            .WithMany(p => p.ActualSchedules)
            .HasForeignKey(d => d.StudentId)
            .HasConstraintName("fk_actual_student");

        builder.HasOne(d => d.Tutor)
            .WithMany(p => p.ActualSchedules)
            .HasForeignKey(d => d.TutorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_actual_tutor");
    }
}

