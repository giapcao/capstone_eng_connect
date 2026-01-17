using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("course_review")]
[Index("EnrollmentId", Name = "uq_review_enrollment", IsUnique = true)]
public partial class CourseReview
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("course_id")]
    public Guid CourseId { get; set; }

    [Column("tutor_id")]
    public Guid TutorId { get; set; }

    [Column("student_id")]
    public Guid StudentId { get; set; }

    [Column("enrollment_id")]
    public Guid EnrollmentId { get; set; }

    [Column("rating")]
    public short? Rating { get; set; }

    [Column("comment")]
    public string? Comment { get; set; }

    [Column("is_anonymous")]
    public bool? IsAnonymous { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("CourseReviews")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("EnrollmentId")]
    [InverseProperty("CourseReview")]
    public virtual CourseEnrollment Enrollment { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("CourseReviews")]
    public virtual Student Student { get; set; } = null!;

    [ForeignKey("TutorId")]
    [InverseProperty("CourseReviews")]
    public virtual Tutor Tutor { get; set; } = null!;
}
