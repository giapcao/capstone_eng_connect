using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class CourseSessionCourseResourceConfiguration: IEntityTypeConfiguration<CourseSessionCourseResource>
{
    public void Configure(EntityTypeBuilder<CourseSessionCourseResource> builder)
    {
        builder.ToTable("course_session_course_resource");
        builder.HasKey(d => d.Id).HasName("course_session_course_resource_pkey");
        
        builder.Property(d => d.Id).
            HasColumnName("id");
        
        builder.Property(d => d.CourseSessionId)
            .HasColumnName("course_session_id");
        builder.Property(d => d.CourseResourceId)
            .HasColumnName("course_resource_id");
        
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
        
        builder.HasIndex(e => new { e.CourseSessionId, e.CourseResourceId })
            .HasDatabaseName("uq_course_session_course_resource")
            .IsUnique();
        builder.HasOne(e => e.CourseSession)
            .WithMany(c => c.CourseSessionCourseResources)
            .HasForeignKey(d => d.CourseSessionId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_cc_course_session");
        
        builder.HasOne(e => e.CourseResource)
            .WithMany(c => c.CourseSessionCourseResources)
            .HasForeignKey(e => e.CourseResourceId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_cc_course_resource");

     
    }
}