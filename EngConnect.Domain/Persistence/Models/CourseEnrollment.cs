using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("course_enrollment")]
[Index("CourseId", "StudentId", Name = "uq_course_student", IsUnique = true)]
public partial class CourseEnrollment
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("course_id")]
    public Guid CourseId { get; set; }

    [Column("student_id")]
    public Guid StudentId { get; set; }

    [Column("price_at_purchase")]
    [Precision(12, 2)]
    public decimal? PriceAtPurchase { get; set; }

    [Column("currency")]
    [StringLength(10)]
    public string? Currency { get; set; }

    [Column("nums_of_session")]
    public int? NumsOfSession { get; set; }

    [Column("status")]
    [StringLength(30)]
    public string? Status { get; set; }

    [Column("enrolled_at")]
    public DateTime? EnrolledAt { get; set; }

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
    [InverseProperty("CourseEnrollments")]
    public virtual Course Course { get; set; } = null!;

    [InverseProperty("Enrollment")]
    public virtual CourseReview? CourseReview { get; set; }

    [InverseProperty("Enrollment")]
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    [ForeignKey("StudentId")]
    [InverseProperty("CourseEnrollments")]
    public virtual Student Student { get; set; } = null!;
}
