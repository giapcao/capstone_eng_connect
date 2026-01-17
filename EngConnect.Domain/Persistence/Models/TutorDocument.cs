using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("tutor_document")]
public partial class TutorDocument
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("tutor_id")]
    public Guid TutorId { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string? Name { get; set; }

    [Column("doc_type")]
    [StringLength(100)]
    public string? DocType { get; set; }

    [Column("url")]
    public string Url { get; set; } = null!;

    [Column("issued_by")]
    [StringLength(255)]
    public string? IssuedBy { get; set; }

    [Column("issued_at")]
    public DateOnly? IssuedAt { get; set; }

    [Column("expired_at")]
    public DateOnly? ExpiredAt { get; set; }

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

    [ForeignKey("TutorId")]
    [InverseProperty("TutorDocuments")]
    public virtual Tutor Tutor { get; set; } = null!;
}
