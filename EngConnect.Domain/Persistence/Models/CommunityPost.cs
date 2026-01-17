using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("community_post")]
public partial class CommunityPost
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("author_id")]
    public Guid AuthorId { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("type")]
    [StringLength(50)]
    public string Type { get; set; } = null!;

    [Column("content")]
    public string Content { get; set; } = null!;

    [Column("is_pinned")]
    public bool? IsPinned { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}
