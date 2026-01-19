using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class CourseVerificationRequestConfiguration : IEntityTypeConfiguration<CourseVerificationRequest>
{
    public void Configure(EntityTypeBuilder<CourseVerificationRequest> builder)
    {
        builder.HasKey(e => e.Id).HasName("course_verification_request_pkey");

        builder.ToTable("course_verification_request");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.Property(e => e.CourseId)
            .HasColumnName("course_id");

        builder.Property(e => e.Status)
            .HasMaxLength(20)
            .HasColumnName("status");

        builder.Property(e => e.SubmittedAt)
            .HasColumnName("submitted_at");

        builder.Property(e => e.ReviewedAt)
            .HasColumnName("reviewed_at");

        builder.Property(e => e.ReviewedBy)
            .HasColumnName("reviewed_by");

        builder.Property(e => e.RejectionReason)
            .HasColumnName("rejection_reason");

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

        builder.HasOne(d => d.Course)
            .WithMany(p => p.CourseVerificationRequests)
            .HasForeignKey(d => d.CourseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_verification_course");

        builder.HasOne(d => d.ReviewedByNavigation)
            .WithMany(p => p.CourseVerificationRequests)
            .HasForeignKey(d => d.ReviewedBy)
            .HasConstraintName("fk_verification_reviewer");
    }
}

