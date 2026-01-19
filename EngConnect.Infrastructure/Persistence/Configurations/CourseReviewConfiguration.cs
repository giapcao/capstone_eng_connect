using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class CourseReviewConfiguration : IEntityTypeConfiguration<CourseReview>
{
    public void Configure(EntityTypeBuilder<CourseReview> builder)
    {
        builder.HasKey(e => e.Id).HasName("course_review_pkey");

        builder.ToTable("course_review");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.Property(e => e.CourseId)
            .HasColumnName("course_id");

        builder.Property(e => e.TutorId)
            .HasColumnName("tutor_id");

        builder.Property(e => e.StudentId)
            .HasColumnName("student_id");

        builder.Property(e => e.EnrollmentId)
            .HasColumnName("enrollment_id");

        builder.Property(e => e.Rating)
            .HasColumnName("rating");

        builder.Property(e => e.Comment)
            .HasColumnName("comment");

        builder.Property(e => e.IsAnonymous)
            .HasColumnName("is_anonymous")
            .HasDefaultValue(false);

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

        builder.HasIndex(e => e.EnrollmentId)
            .HasDatabaseName("uq_review_enrollment")
            .IsUnique();

        builder.HasOne(d => d.Course)
            .WithMany(p => p.CourseReviews)
            .HasForeignKey(d => d.CourseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_review_course");

        builder.HasOne(d => d.Enrollment)
            .WithOne(p => p.CourseReview)
            .HasForeignKey<CourseReview>(d => d.EnrollmentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_review_enrollment");

        builder.HasOne(d => d.Student)
            .WithMany(p => p.CourseReviews)
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_review_student");

        builder.HasOne(d => d.Tutor)
            .WithMany(p => p.CourseReviews)
            .HasForeignKey(d => d.TutorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_review_tutor");
    }
}

