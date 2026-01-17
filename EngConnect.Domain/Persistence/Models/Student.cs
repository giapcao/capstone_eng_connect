using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("student")]
[Index("UserId", Name = "uq_student_user", IsUnique = true)]
public partial class Student
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("school")]
    [StringLength(255)]
    public string? School { get; set; }

    [Column("grade")]
    [StringLength(50)]
    public string? Grade { get; set; }

    [Column("class")]
    [StringLength(50)]
    public string? Class { get; set; }

    [Column("tags")]
    public List<string>? Tags { get; set; }

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

    [InverseProperty("Student")]
    public virtual ICollection<ActualSchedule> ActualSchedules { get; set; } = new List<ActualSchedule>();

    [InverseProperty("Student")]
    public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

    [InverseProperty("Student")]
    public virtual ICollection<CourseEnrollment> CourseEnrollments { get; set; } = new List<CourseEnrollment>();

    [InverseProperty("Student")]
    public virtual ICollection<CourseReview> CourseReviews { get; set; } = new List<CourseReview>();

    [InverseProperty("Student")]
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    [ForeignKey("UserId")]
    [InverseProperty("Student")]
    public virtual User User { get; set; } = null!;
}
