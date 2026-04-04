using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class CourseModuleConfiguration : IEntityTypeConfiguration<CourseModule>
{
    public void Configure(EntityTypeBuilder<CourseModule> builder)
    {
        builder.HasKey(e => e.Id).HasName("course_module_pkey");

        builder.ToTable("course_module");

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.TutorId)
            .HasColumnName("tutor_id");

        builder.Property(e => e.ParentModuleId)
            .HasColumnName("parent_module_id");

        builder.Property(e => e.Title)
            .HasMaxLength(255)
            .HasColumnName("title");
        
        builder.Property(e => e.Description)
            .HasColumnName("description");

        builder.Property(e => e.Outcomes)
            .HasColumnName("outcomes");

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
            .WithMany(p => p.CourseModules)
            .HasForeignKey(d => d.TutorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_module_tutor");

        builder.HasOne(d => d.ParentModule)
            .WithMany(p => p.InverseParentModule)
            .HasForeignKey(d => d.ParentModuleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_course_module_parent");
    }
}

