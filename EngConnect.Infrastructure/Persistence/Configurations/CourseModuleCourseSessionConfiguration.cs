using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class CourseModuleCourseSessionConfiguratiom : IEntityTypeConfiguration<CourseModuleCourseSession>
{
    public void Configure(EntityTypeBuilder<CourseModuleCourseSession> builder)
    {
        builder.ToTable("course_module_course_session");
        builder.HasKey(d => d.Id)
            .HasName("course_module_course_session_pkey");
        builder.Property(d => d.Id)
            .HasColumnName("id");
        builder.Property(d => d.CourseModuleId)
            .HasColumnName("course_module_id");
        builder.Property(d => d.CourseSessionId)
            .HasColumnName("course_session_id");
        builder.Property(d => d.SessionNumber)
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


        builder.HasIndex(e => new { e.CourseModuleId, e.CourseSessionId })
            .HasDatabaseName("uq_course_module_course_session")
            .IsUnique();

        builder.HasOne(e => e.CourseModule)
            .WithMany(c => c.CourseModuleCourseSessions)
            .HasForeignKey(e => e.CourseModuleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_cc_course_module");

        builder.HasOne(e => e.CourseSession)
            .WithMany(c => c.CourseModuleCourseSessions)
            .HasForeignKey(d => d.CourseSessionId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_cc_course_session");
    }
}