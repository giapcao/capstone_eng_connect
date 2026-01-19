using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class LessonScriptConfiguration : IEntityTypeConfiguration<LessonScript>
{
    public void Configure(EntityTypeBuilder<LessonScript> builder)
    {
        builder.HasKey(e => e.Id).HasName("lesson_script_pkey");

        builder.ToTable("lesson_script");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.Property(e => e.LessonId)
            .HasColumnName("lesson_id");

        builder.Property(e => e.RecordId)
            .HasColumnName("record_id");

        builder.Property(e => e.Language)
            .HasMaxLength(20)
            .HasColumnName("language");

        builder.Property(e => e.FullText)
            .HasColumnName("full_text");

        builder.Property(e => e.SummarizeText)
            .HasColumnName("summarize_text");

        builder.Property(e => e.LessonOutcome)
            .HasColumnName("lesson_outcome");

        builder.Property(e => e.CoveragePercent)
            .HasPrecision(5, 2)
            .HasColumnName("coverage_percent");

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

        builder.HasOne(d => d.Lesson)
            .WithMany(p => p.LessonScripts)
            .HasForeignKey(d => d.LessonId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_script_lesson");

        builder.HasOne(d => d.Record)
            .WithMany(p => p.LessonScripts)
            .HasForeignKey(d => d.RecordId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_script_record");
    }
}

