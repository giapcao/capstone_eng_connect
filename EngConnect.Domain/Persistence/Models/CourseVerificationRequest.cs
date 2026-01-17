using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("course_verification_request")]
public partial class CourseVerificationRequest
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("course_id")]
    public Guid CourseId { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string? Status { get; set; }

    [Column("submitted_at")]
    public DateTime? SubmittedAt { get; set; }

    [Column("reviewed_at")]
    public DateTime? ReviewedAt { get; set; }

    [Column("reviewed_by")]
    public Guid? ReviewedBy { get; set; }

    [Column("rejection_reason")]
    public string? RejectionReason { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("CourseVerificationRequests")]
    public virtual Course Course { get; set; } = null!;

    [ForeignKey("ReviewedBy")]
    [InverseProperty("CourseVerificationRequests")]
    public virtual User? ReviewedByNavigation { get; set; }
}
