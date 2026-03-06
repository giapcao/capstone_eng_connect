using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(e => e.Id).HasName("course_pkey");

        builder.ToTable("course");

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.TutorId)
            .HasColumnName("tutor_id");

        builder.Property(e => e.ParentCourseId)
            .HasColumnName("parent_course_id");

        builder.Property(e => e.Title)
            .HasMaxLength(255)
            .HasColumnName("title");

        builder.Property(e => e.ShortDescription)
            .HasColumnName("short_description");

        builder.Property(e => e.FullDescription)
            .HasColumnName("full_description");

        builder.Property(e => e.Outcomes)
            .HasColumnName("outcomes");

        builder.Property(e => e.Level)
            .HasMaxLength(50)
            .HasColumnName("level");

        builder.Property(e => e.EstimatedTime)
            .HasColumnName("estimated_time");

        builder.Property(e => e.EstimatedTimeLesson)
            .HasColumnName("estimated_time_lesson");

        builder.Property(e => e.Price)
            .HasPrecision(12, 2)
            .HasColumnName("price");

        builder.Property(e => e.Currency)
            .HasMaxLength(10)
            .HasColumnName("currency");

        builder.Property(e => e.NumberOfSessions)
            .HasColumnName("number_of_sessions");

        builder.Property(e => e.NumsSessionInWeek)
            .HasColumnName("nums_session_in_week");

        builder.Property(e => e.ThumbnailUrl)
            .HasColumnName("thumbnail_url");

        builder.Property(e => e.DemoVideoUrl)
            .HasColumnName("demo_video_url");

        builder.Property(e => e.NumberOfEnrollment)
            .HasColumnName("number_of_enrollment")
            .HasDefaultValue(0);

        builder.Property(e => e.RatingAverage)
            .HasPrecision(3, 2)
            .HasColumnName("rating_average")
            .HasDefaultValueSql("0");

        builder.Property(e => e.RatingCount)
            .HasColumnName("rating_count")
            .HasDefaultValue(0);

        builder.Property(e => e.Status)
            .HasMaxLength(30)
            .HasColumnName("status");

        builder.Property(e => e.IsCertificate)
            .HasColumnName("is_certificate")
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

        builder.HasOne(d => d.Tutor)
            .WithMany(p => p.Courses)
            .HasForeignKey(d => d.TutorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_course_tutor");

        builder.HasOne(d => d.ParentCourse)
            .WithMany(p => p.InverseParentCourse)
            .HasForeignKey(d => d.ParentCourseId)
            .HasConstraintName("fk_course_parent");
    }
}

