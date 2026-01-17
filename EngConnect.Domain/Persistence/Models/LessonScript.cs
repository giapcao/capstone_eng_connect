using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("lesson_script")]
public partial class LessonScript
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("lesson_id")]
    public Guid LessonId { get; set; }

    [Column("record_id")]
    public Guid RecordId { get; set; }

    [Column("language")]
    [StringLength(20)]
    public string? Language { get; set; }

    [Column("full_text")]
    public string? FullText { get; set; }

    [Column("summarize_text")]
    public string? SummarizeText { get; set; }

    [Column("lesson_outcome")]
    public string? LessonOutcome { get; set; }

    [Column("coverage_percent")]
    [Precision(5, 2)]
    public decimal? CoveragePercent { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("LessonId")]
    [InverseProperty("LessonScripts")]
    public virtual Lesson Lesson { get; set; } = null!;

    [ForeignKey("RecordId")]
    [InverseProperty("LessonScripts")]
    public virtual LessonRecord Record { get; set; } = null!;
}
