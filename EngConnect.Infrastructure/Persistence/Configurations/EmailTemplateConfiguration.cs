using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder.HasKey(e => e.Id).HasName("email_template_pkey");

        builder.ToTable("email_template");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .HasColumnName("name");

        builder.Property(e => e.SubjectTemplate)
            .HasColumnName("subject_template");

        builder.Property(e => e.BodyHtmlTemplate)
            .HasColumnName("body_html_template");

        builder.Property(e => e.BodyTextTemplate)
            .HasColumnName("body_text_template");

        builder.Property(e => e.UpdatedBy)
            .HasColumnName("updated_by");

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

        builder.HasOne(d => d.UpdatedByNavigation)
            .WithMany(p => p.EmailTemplates)
            .HasForeignKey(d => d.UpdatedBy)
            .HasConstraintName("fk_email_template_updated_by");
    }
}

