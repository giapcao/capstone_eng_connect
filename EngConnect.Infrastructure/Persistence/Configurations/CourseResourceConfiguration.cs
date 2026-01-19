using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class CourseResourceConfiguration : IEntityTypeConfiguration<CourseResource>
{
    public void Configure(EntityTypeBuilder<CourseResource> builder)
    {
        builder.HasKey(e => e.Id).HasName("course_resource_pkey");

        builder.ToTable("course_resource");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.Property(e => e.SessionId)
            .HasColumnName("session_id");

        builder.Property(e => e.Title)
            .HasMaxLength(255)
            .HasColumnName("title");

        builder.Property(e => e.ResourceType)
            .HasMaxLength(50)
            .HasColumnName("resource_type");

        builder.Property(e => e.Url)
            .HasColumnName("url");

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

        builder.HasOne(d => d.Session)
            .WithMany(p => p.CourseResources)
            .HasForeignKey(d => d.SessionId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_resource_session");
    }
}

