using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class QuizAttemptAnswerConfiguration : IEntityTypeConfiguration<QuizAttemptAnswer>
{
    public void Configure(EntityTypeBuilder<QuizAttemptAnswer> builder)
    {
        builder.HasKey(e => e.Id).HasName("quiz_attempt_answer_pkey");

        builder.ToTable("quiz_attempt_answer");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(e => e.AttemptId)
            .HasColumnName("attempt_id");

        builder.Property(e => e.QuestionId)
            .HasColumnName("question_id");

        builder.Property(e => e.Answer)
            .HasColumnName("answer");

        builder.Property(e => e.IsCorrect)
            .HasColumnName("is_correct");

        builder.Property(e => e.ReceivePoint)
            .HasPrecision(5, 2)
            .HasColumnName("receive_point");

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
    }
}

