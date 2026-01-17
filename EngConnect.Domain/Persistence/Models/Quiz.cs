using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("quiz")]
public partial class Quiz
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("course_id")]
    public Guid CourseId { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("is_open")]
    public bool? IsOpen { get; set; }

    [Column("max_score")]
    public int MaxScore { get; set; }

    [Column("duration_seconds")]
    public int DurationSeconds { get; set; }

    [Column("attempt_limit")]
    public int? AttemptLimit { get; set; }

    [Column("expired_at")]
    public DateTime? ExpiredAt { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Quizzes")]
    public virtual Course Course { get; set; } = null!;

    [InverseProperty("Quiz")]
    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
}
