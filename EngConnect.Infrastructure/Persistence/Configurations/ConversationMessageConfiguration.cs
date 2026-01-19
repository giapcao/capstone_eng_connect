using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class ConversationMessageConfiguration : IEntityTypeConfiguration<ConversationMessage>
{
    public void Configure(EntityTypeBuilder<ConversationMessage> builder)
    {
        builder.HasKey(e => e.Id).HasName("conversation_message_pkey");

        builder.ToTable("conversation_message");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.ConversationId)
            .HasColumnName("conversation_id");

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

        builder.HasOne(d => d.Conversation)
            .WithMany(p => p.ConversationMessages)
            .HasForeignKey(d => d.ConversationId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_conversation_message");
    }
}

