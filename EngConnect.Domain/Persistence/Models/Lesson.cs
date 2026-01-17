using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("lesson")]
public partial class Lesson
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("tutor_id")]
    public Guid TutorId { get; set; }

    [Column("student_id")]
    public Guid StudentId { get; set; }

    [Column("enrollment_id")]
    public Guid EnrollmentId { get; set; }

    [Column("session_id")]
    public Guid? SessionId { get; set; }

    [Column("start_time")]
    public DateTime? StartTime { get; set; }

    [Column("end_time")]
    public DateTime? EndTime { get; set; }

    [Column("status")]
    [StringLength(30)]
    public string? Status { get; set; }

    [Column("meeting_url")]
    public string? MeetingUrl { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("EnrollmentId")]
    [InverseProperty("Lessons")]
    public virtual CourseEnrollment Enrollment { get; set; } = null!;

    [InverseProperty("Lesson")]
    public virtual ICollection<LessonHomework> LessonHomeworks { get; set; } = new List<LessonHomework>();

    [InverseProperty("Lesson")]
    public virtual ICollection<LessonRecord> LessonRecords { get; set; } = new List<LessonRecord>();

    [InverseProperty("Lesson")]
    public virtual ICollection<LessonScript> LessonScripts { get; set; } = new List<LessonScript>();

    [ForeignKey("SessionId")]
    [InverseProperty("Lessons")]
    public virtual CourseSession? Session { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("Lessons")]
    public virtual Student Student { get; set; } = null!;

    [ForeignKey("TutorId")]
    [InverseProperty("Lessons")]
    public virtual Tutor Tutor { get; set; } = null!;
}
