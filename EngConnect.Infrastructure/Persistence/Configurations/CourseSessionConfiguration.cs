using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class CourseSessionConfiguration : IEntityTypeConfiguration<CourseSession>
{
    public void Configure(EntityTypeBuilder<CourseSession> builder)
    {
        builder.HasKey(e => e.Id).HasName("course_session_pkey");

        builder.ToTable("course_session");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.Property(e => e.ModuleId)
            .HasColumnName("module_id");

        builder.Property(e => e.Title)
            .HasMaxLength(255)
            .HasColumnName("title");

        builder.Property(e => e.Description)
            .HasColumnName("description");

        builder.Property(e => e.Outcomes)
            .HasColumnName("outcomes");

        builder.Property(e => e.SessionNumber)
            .HasColumnName("session_number");

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

        builder.HasOne(d => d.Module)
            .WithMany(p => p.CourseSessions)
            .HasForeignKey(d => d.ModuleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_session_module");
    }
}

