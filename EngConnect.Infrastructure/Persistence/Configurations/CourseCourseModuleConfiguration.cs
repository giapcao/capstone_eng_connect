using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class CourseCourseModuleConfiguration: IEntityTypeConfiguration<CourseCourseModule>
{
    public void Configure(EntityTypeBuilder<CourseCourseModule> builder)
    {
        builder.ToTable("course_course_module");
        
        builder.HasKey(e => e.Id).HasName("course_course_module_pkey");

        builder.Property(e => e.Id)
            .HasColumnName("id");
        builder.Property(e => e.CourseId)
            .HasColumnName("course_id");
        builder.Property(e => e.CourseModuleId) 
            .HasColumnName("course_module_id");
        
        builder.Property(e => e.ModuleNumber)
            .HasColumnName("module_number");
        
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
        
        
        builder.HasIndex(e => new { e.CourseId, e.CourseModuleId })
            .HasDatabaseName("uq_course_course_module")
            .IsUnique();
        
        builder.HasOne(e => e.Course)
            .WithMany(c => c.CourseCourseModules)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_cc_course");
        
        builder.HasOne(e => e.CourseModule)
            .WithMany(c => c.CourseCourseModules)
            .HasForeignKey(d => d.CourseModuleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_cc_course_module");
    }
}