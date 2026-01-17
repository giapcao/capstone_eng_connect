using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("tutor_verification_request")]
public partial class TutorVerificationRequest
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("tutor_id")]
    public Guid TutorId { get; set; }

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

    [ForeignKey("ReviewedBy")]
    [InverseProperty("TutorVerificationRequests")]
    public virtual User? ReviewedByNavigation { get; set; }

    [ForeignKey("TutorId")]
    [InverseProperty("TutorVerificationRequests")]
    public virtual Tutor Tutor { get; set; } = null!;
}
