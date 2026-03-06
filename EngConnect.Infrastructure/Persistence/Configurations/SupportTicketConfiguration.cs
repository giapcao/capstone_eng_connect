using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
{
    public void Configure(EntityTypeBuilder<SupportTicket> builder)
    {
        builder.HasKey(e => e.Id).HasName("support_ticket_pkey");

        builder.ToTable("support_ticket");

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by");

        builder.Property(e => e.Subject)
            .HasMaxLength(255)
            .HasColumnName("subject");

        builder.Property(e => e.Description)
            .HasColumnName("description");

        builder.Property(e => e.Type)
            .HasMaxLength(50)
            .HasColumnName("type");

        builder.Property(e => e.Priority)
            .HasMaxLength(20)
            .HasColumnName("priority");

        builder.Property(e => e.Status)
            .HasMaxLength(30)
            .HasColumnName("status");

        builder.Property(e => e.ClosedAt)
            .HasColumnName("closed_at");

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

        builder.HasOne(d => d.CreatedByNavigation)
            .WithMany(p => p.SupportTickets)
            .HasForeignKey(d => d.CreatedBy)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_ticket_created_by");
    }
}

