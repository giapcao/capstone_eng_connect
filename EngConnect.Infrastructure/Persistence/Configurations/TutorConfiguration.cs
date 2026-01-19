using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class TutorConfiguration : IEntityTypeConfiguration<Tutor>
{
    public void Configure(EntityTypeBuilder<Tutor> builder)
    {
        builder.HasKey(e => e.Id).HasName("tutor_pkey");

        builder.ToTable("tutor");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.Property(e => e.UserId)
            .HasColumnName("user_id");

        builder.Property(e => e.Headline)
            .HasMaxLength(255)
            .HasColumnName("headline");

        builder.Property(e => e.Bio)
            .HasColumnName("bio");

        builder.Property(e => e.IntroVideoUrl)
            .HasColumnName("intro_video_url");

        builder.Property(e => e.YearsExperience)
            .HasColumnName("years_experience");

        builder.Property(e => e.CvUrl)
            .HasColumnName("cv_url");

        builder.Property(e => e.Tags)
            .HasColumnName("tags")
            .HasColumnType("text[]");

        builder.Property(e => e.SlotsCount)
            .HasColumnName("slots_count")
            .HasDefaultValue(0);

        builder.Property(e => e.Status)
            .HasMaxLength(20)
            .HasColumnName("status");

        builder.Property(e => e.VerifiedStatus)
            .HasMaxLength(20)
            .HasColumnName("verified_status");

        builder.Property(e => e.RatingAverage)
            .HasPrecision(3, 2)
            .HasColumnName("rating_average")
            .HasDefaultValueSql("0");

        builder.Property(e => e.RatingCount)
            .HasColumnName("rating_count")
            .HasDefaultValue(0);

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
            .HasDatabaseName("uq_tutor_user")
            .IsUnique();

        builder.HasOne(d => d.User)
            .WithOne(p => p.Tutor)
            .HasForeignKey<Tutor>(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_tutor_user");
    }
}

