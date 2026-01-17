using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("course_resource")]
public partial class CourseResource
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("session_id")]
    public Guid SessionId { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string? Title { get; set; }

    [Column("resource_type")]
    [StringLength(50)]
    public string? ResourceType { get; set; }

    [Column("url")]
    public string Url { get; set; } = null!;

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

    [ForeignKey("SessionId")]
    [InverseProperty("CourseResources")]
    public virtual CourseSession Session { get; set; } = null!;
}
