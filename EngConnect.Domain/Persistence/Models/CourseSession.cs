using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("course_session")]
public partial class CourseSession
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("module_id")]
    public Guid ModuleId { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string? Title { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("outcomes")]
    public string? Outcomes { get; set; }

    [Column("session_number")]
    public int? SessionNumber { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [InverseProperty("Session")]
    public virtual ICollection<CourseResource> CourseResources { get; set; } = new List<CourseResource>();

    [InverseProperty("Session")]
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    [ForeignKey("ModuleId")]
    [InverseProperty("CourseSessions")]
    public virtual CourseModule Module { get; set; } = null!;
}
