using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("conversation_message")]
public partial class ConversationMessage
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("conversation_id")]
    public Guid ConversationId { get; set; }

    [Column("sender_id")]
    public Guid SenderId { get; set; }

    [Column("message")]
    public string Message { get; set; } = null!;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("ConversationId")]
    [InverseProperty("ConversationMessages")]
    public virtual Conversation Conversation { get; set; } = null!;
}
