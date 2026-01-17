using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("community_comment")]
public partial class CommunityComment
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("post_id")]
    public Guid PostId { get; set; }

    [Column("author_id")]
    public Guid AuthorId { get; set; }

    [Column("parent_comment_id")]
    public Guid? ParentCommentId { get; set; }

    [Column("content")]
    public string Content { get; set; } = null!;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
}
