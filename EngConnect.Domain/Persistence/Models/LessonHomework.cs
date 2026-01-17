using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("lesson_homework")]
public partial class LessonHomework
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("lesson_id")]
    public Guid LessonId { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("resource_url")]
    public string? ResourceUrl { get; set; }

    [Column("submission_url")]
    public string? SubmissionUrl { get; set; }

    [Column("score")]
    [Precision(5, 2)]
    public decimal? Score { get; set; }

    [Column("max_score")]
    [Precision(5, 2)]
    public decimal MaxScore { get; set; }

    [Column("tutor_feedback")]
    public string? TutorFeedback { get; set; }

    [Column("assigned_at")]
    public DateTime? AssignedAt { get; set; }

    [Column("submitted_at")]
    public DateTime? SubmittedAt { get; set; }

    [Column("due_at")]
    public DateTime? DueAt { get; set; }

    [Column("status")]
    [StringLength(30)]
    public string Status { get; set; } = null!;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("LessonId")]
    [InverseProperty("LessonHomeworks")]
    public virtual Lesson Lesson { get; set; } = null!;
}
