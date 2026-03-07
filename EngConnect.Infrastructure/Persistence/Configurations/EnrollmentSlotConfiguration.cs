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
    public class EnrollmentSlotConfiguration : IEntityTypeConfiguration<EnrollmentSlot>
    {
        public void Configure(EntityTypeBuilder<EnrollmentSlot> builder)
        {
            builder.HasKey(e => e.Id).HasName("enrollment_slot_pkey");

            builder.ToTable("enrollment_slot");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.EnrollmentId)
                .HasColumnName("enrollment_id");

            builder.Property(e => e.TutorId)
                .HasColumnName("tutor_id");

            builder.Property(e => e.Weekday)
                .HasMaxLength(20)
                .HasColumnName("weekday");

            builder.Property(e => e.StartTime)
                .HasColumnName("start_time");

            builder.Property(e => e.EndTime)
                .HasColumnName("end_time");

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

            builder.HasIndex(e => new { e.TutorId, e.Weekday, e.StartTime, e.EndTime })
                .HasDatabaseName("uq_locked_slot")
                .IsUnique();

            builder.HasOne(d => d.Enrollment)
                .WithMany(p => p.EnrollmentSlots)
                .HasForeignKey(d => d.EnrollmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_es_enrollment");

            builder.HasOne(d => d.Tutor)
                .WithMany(p => p.EnrollmentSlots)
                .HasForeignKey(d => d.TutorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_es_tutor");
        }
    }
}