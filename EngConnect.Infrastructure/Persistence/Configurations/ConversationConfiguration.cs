using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.HasKey(e => e.Id).HasName("conversation_pkey");

        builder.ToTable("conversation");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.TutorId)
            .HasColumnName("tutor_id");

        builder.Property(e => e.StudentId)
            .HasColumnName("student_id");

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

        builder.HasIndex(e => new { e.TutorId, e.StudentId })
            .HasDatabaseName("uq_conversation_tutor_student")
            .IsUnique();

        builder.HasOne(d => d.Student)
            .WithMany(p => p.Conversations)
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_conversation_student");

        builder.HasOne(d => d.Tutor)
            .WithMany(p => p.Conversations)
            .HasForeignKey(d => d.TutorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_conversation_tutor");
    }
}

