using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class TutorDocumentConfiguration : IEntityTypeConfiguration<TutorDocument>
{
    public void Configure(EntityTypeBuilder<TutorDocument> builder)
    {
        builder.HasKey(e => e.Id).HasName("tutor_document_pkey");

        builder.ToTable("tutor_document");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.Property(e => e.TutorId)
            .HasColumnName("tutor_id");

        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .HasColumnName("name");

        builder.Property(e => e.DocType)
            .HasMaxLength(100)
            .HasColumnName("doc_type");

        builder.Property(e => e.Url)
            .HasColumnName("url");

        builder.Property(e => e.IssuedBy)
            .HasMaxLength(255)
            .HasColumnName("issued_by");

        builder.Property(e => e.IssuedAt)
            .HasColumnName("issued_at");

        builder.Property(e => e.ExpiredAt)
            .HasColumnName("expired_at");

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

        builder.HasOne(d => d.Tutor)
            .WithMany(p => p.TutorDocuments)
            .HasForeignKey(d => d.TutorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_document_tutor");
    }
}

