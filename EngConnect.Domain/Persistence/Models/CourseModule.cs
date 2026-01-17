using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("course_module")]
public partial class CourseModule
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("course_id")]
    public Guid CourseId { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("outcomes")]
    public string? Outcomes { get; set; }

    [Column("module_number")]
    public int? ModuleNumber { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("CourseModules")]
    public virtual Course Course { get; set; } = null!;

    [InverseProperty("Module")]
    public virtual ICollection<CourseSession> CourseSessions { get; set; } = new List<CourseSession>();
}
