using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("course_category")]
[Index("CourseId", "CategoryId", Name = "uq_course_category", IsUnique = true)]
public partial class CourseCategory
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("course_id")]
    public Guid CourseId { get; set; }

    [Column("category_id")]
    public Guid CategoryId { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("CourseCategories")]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("CourseId")]
    [InverseProperty("CourseCategories")]
    public virtual Course Course { get; set; } = null!;
}
