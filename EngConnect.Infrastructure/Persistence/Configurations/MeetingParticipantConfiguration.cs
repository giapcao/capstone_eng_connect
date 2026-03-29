using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class MeetingParticipantConfiguration : IEntityTypeConfiguration<MeetingParticipant>
{
    public void Configure(EntityTypeBuilder<MeetingParticipant> builder)
    {
        builder.HasKey(e => e.Id).HasName("meeting_participant_pkey");

        builder.ToTable("meeting_participant");

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.LessonId)
            .HasColumnName("lesson_id");

        builder.Property(e => e.UserId)
            .HasColumnName("user_id");

        builder.Property(e => e.Role)
            .HasMaxLength(30)
            .HasColumnName("role");

        builder.Property(e => e.JoinedAt)
            .HasColumnName("joined_at");

        builder.Property(e => e.LeftAt)
            .HasColumnName("left_at");

        builder.Property(e => e.ConnectionId)
            .HasMaxLength(255)
            .HasColumnName("connection_id");

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
            .WithMany(p => p.MeetingParticipants)
            .HasForeignKey(d => d.LessonId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_meeting_participant_lesson");

        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_meeting_participant_user");
    }
}