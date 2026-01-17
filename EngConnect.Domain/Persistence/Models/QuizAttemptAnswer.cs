using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("quiz_attempt_answer")]
public partial class QuizAttemptAnswer
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("attempt_id")]
    public Guid AttemptId { get; set; }

    [Column("question_id")]
    public Guid QuestionId { get; set; }

    [Column("answer")]
    public string? Answer { get; set; }

    [Column("is_correct")]
    public bool? IsCorrect { get; set; }

    [Column("receive_point")]
    [Precision(5, 2)]
    public decimal? ReceivePoint { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}
