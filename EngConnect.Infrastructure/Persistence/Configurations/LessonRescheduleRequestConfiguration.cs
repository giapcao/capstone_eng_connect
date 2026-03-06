using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Infrastructure.Persistence.Configurations
{
    public class LessonRescheduleRequestConfiguration : IEntityTypeConfiguration<LessonRescheduleRequest>
    {
        public void Configure(EntityTypeBuilder<LessonRescheduleRequest> builder)
        {
            builder.HasKey(e => e.Id).HasName("lesson_reschedule_request_pkey");

            builder.ToTable("lesson_reschedule_request");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.LessonId)
                .HasColumnName("lesson_id");

            builder.Property(e => e.StudentId)
                .HasColumnName("student_id");

            builder.Property(e => e.ProposedStartTime)
                .HasColumnName("proposed_start_time");

            builder.Property(e => e.ProposedEndTime)
                .HasColumnName("proposed_end_time");

            builder.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status")
                .HasDefaultValueSql("'pending'::character varying");

            builder.Property(e => e.TutorNote)
                .HasColumnName("tutor_note");

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
                .WithMany(p => p.LessonRescheduleRequests)
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_lrr_lesson");

            builder.HasOne(d => d.Student)
                .WithMany(p => p.LessonRescheduleRequests)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_lrr_student");
        }
    }
}