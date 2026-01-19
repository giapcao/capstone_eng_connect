using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class SupportTicketMessageConfiguration : IEntityTypeConfiguration<SupportTicketMessage>
{
    public void Configure(EntityTypeBuilder<SupportTicketMessage> builder)
    {
        builder.HasKey(e => e.Id).HasName("support_ticket_message_pkey");

        builder.ToTable("support_ticket_message");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.TicketId)
            .HasColumnName("ticket_id");

        builder.Property(e => e.SenderId)
            .HasColumnName("sender_id");

        builder.Property(e => e.Message)
            .HasColumnName("message");

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

        builder.HasOne(d => d.Sender)
            .WithMany(p => p.SupportTicketMessages)
            .HasForeignKey(d => d.SenderId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_ticket_message_sender");

        builder.HasOne(d => d.Ticket)
            .WithMany(p => p.SupportTicketMessages)
            .HasForeignKey(d => d.TicketId)
            .HasConstraintName("fk_ticket_message_ticket");
    }
}

