using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("support_ticket_message")]
public partial class SupportTicketMessage
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("ticket_id")]
    public Guid TicketId { get; set; }

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

    [ForeignKey("SenderId")]
    [InverseProperty("SupportTicketMessages")]
    public virtual User Sender { get; set; } = null!;

    [ForeignKey("TicketId")]
    [InverseProperty("SupportTicketMessages")]
    public virtual SupportTicket Ticket { get; set; } = null!;
}
