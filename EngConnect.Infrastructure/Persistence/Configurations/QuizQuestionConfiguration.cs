using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
{
    public void Configure(EntityTypeBuilder<QuizQuestion> builder)
    {
        builder.HasKey(e => e.Id).HasName("quiz_question_pkey");

        builder.ToTable("quiz_question");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(e => e.QuizId)
            .HasColumnName("quiz_id");

        builder.Property(e => e.QuestionType)
            .HasMaxLength(30)
            .HasColumnName("question_type");

        builder.Property(e => e.QuestionNumber)
            .HasColumnName("question_number");

        builder.Property(e => e.QuestionText)
            .HasColumnName("question_text");

        builder.Property(e => e.Options)
            .HasColumnType("json")
            .HasColumnName("options");

        builder.Property(e => e.CorrectAnswer)
            .HasColumnName("correct_answer");

        builder.Property(e => e.Point)
            .HasColumnName("point");

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

        builder.HasOne(d => d.Quiz)
            .WithMany(p => p.QuizQuestions)
            .HasForeignKey(d => d.QuizId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_quiz_quizquestion");
    }
}

