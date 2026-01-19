using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class CourseEnrollmentConfiguration : IEntityTypeConfiguration<CourseEnrollment>
{
    public void Configure(EntityTypeBuilder<CourseEnrollment> builder)
    {
        builder.HasKey(e => e.Id).HasName("course_enrollment_pkey");

        builder.ToTable("course_enrollment");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.Property(e => e.CourseId)
            .HasColumnName("course_id");

        builder.Property(e => e.StudentId)
            .HasColumnName("student_id");

        builder.Property(e => e.PriceAtPurchase)
            .HasPrecision(12, 2)
            .HasColumnName("price_at_purchase");

        builder.Property(e => e.Currency)
            .HasMaxLength(10)
            .HasColumnName("currency");

        builder.Property(e => e.NumsOfSession)
            .HasColumnName("nums_of_session");

        builder.Property(e => e.Status)
            .HasMaxLength(30)
            .HasColumnName("status");

        builder.Property(e => e.EnrolledAt)
            .HasColumnName("enrolled_at");

        builder.Property(e => e.ExpiredAt)
            .HasColumnName("expired_at");

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

        builder.HasIndex(e => new { e.CourseId, e.StudentId })
            .HasDatabaseName("uq_course_student")
            .IsUnique();

        builder.HasOne(d => d.Course)
            .WithMany(p => p.CourseEnrollments)
            .HasForeignKey(d => d.CourseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_enrollment_course");

        builder.HasOne(d => d.Student)
            .WithMany(p => p.CourseEnrollments)
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_enrollment_student");
    }
}

