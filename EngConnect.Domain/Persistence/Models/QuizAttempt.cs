using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("quiz_attempt")]
public partial class QuizAttempt
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("quiz_id")]
    public Guid QuizId { get; set; }

    [Column("student_id")]
    public Guid StudentId { get; set; }

    [Column("started_at")]
    public DateTime StartedAt { get; set; }

    [Column("completed_at")]
    public DateTime? CompletedAt { get; set; }

    [Column("score")]
    [Precision(5, 2)]
    public decimal? Score { get; set; }

    [Column("duration_seconds")]
    public int? DurationSeconds { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string? Status { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}
