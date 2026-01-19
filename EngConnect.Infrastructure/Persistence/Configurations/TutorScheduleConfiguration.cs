using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class TutorScheduleConfiguration : IEntityTypeConfiguration<TutorSchedule>
{
    public void Configure(EntityTypeBuilder<TutorSchedule> builder)
    {
        builder.HasKey(e => e.Id).HasName("tutor_schedule_pkey");

        builder.ToTable("tutor_schedule");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

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

        builder.HasOne(d => d.Tutor)
            .WithMany(p => p.TutorSchedules)
            .HasForeignKey(d => d.TutorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_schedule_tutor");
    }
}

