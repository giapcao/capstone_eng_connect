using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("quiz_question")]
public partial class QuizQuestion
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("quiz_id")]
    public Guid QuizId { get; set; }

    [Column("question_type")]
    [StringLength(30)]
    public string QuestionType { get; set; } = null!;

    [Column("question_number")]
    public int QuestionNumber { get; set; }

    [Column("question_text")]
    public string QuestionText { get; set; } = null!;

    [Column("options", TypeName = "json")]
    public string? Options { get; set; }

    [Column("correct_answer")]
    public string? CorrectAnswer { get; set; }

    [Column("point")]
    public int Point { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("QuizId")]
    [InverseProperty("QuizQuestions")]
    public virtual Quiz Quiz { get; set; } = null!;
}
