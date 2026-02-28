using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(e => e.Id).HasName("student_pkey");

        builder.ToTable("student");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.Property(e => e.UserId)
            .HasColumnName("user_id");

        builder.Property(e => e.Notes)
            .HasColumnName("notes");

        builder.Property(e => e.School)
            .HasMaxLength(255)
            .HasColumnName("school");

        builder.Property(e => e.Grade)
            .HasMaxLength(50)
            .HasColumnName("grade");

        builder.Property(e => e.Class)
            .HasMaxLength(50)
            .HasColumnName("class");

        builder.Property(e => e.Tags)
            .HasColumnName("tags")
            .HasColumnType("text[]");

        builder.Property(e => e.Status)
            .HasMaxLength(20)
            .HasColumnName("status");

        builder.Property(e=>e.Avatar)
            .HasMaxLength(255)
            .HasColumnName("avatar");
        
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

        builder.HasIndex(e => e.UserId)
            .HasDatabaseName("uq_student_user")
            .IsUnique();

        builder.HasOne(d => d.User)
            .WithOne(p => p.Student)
            .HasForeignKey<Student>(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_student_user");
    }
}

