using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("lesson_record")]
public partial class LessonRecord
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("lesson_id")]
    public Guid LessonId { get; set; }

    [Column("record_url")]
    public string RecordUrl { get; set; } = null!;

    [Column("duration_seconds")]
    public int? DurationSeconds { get; set; }

    [Column("recording_started_at")]
    public DateTime? RecordingStartedAt { get; set; }

    [Column("recording_ended_at")]
    public DateTime? RecordingEndedAt { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("LessonId")]
    [InverseProperty("LessonRecords")]
    public virtual Lesson Lesson { get; set; } = null!;

    [InverseProperty("Record")]
    public virtual ICollection<LessonScript> LessonScripts { get; set; } = new List<LessonScript>();
}
