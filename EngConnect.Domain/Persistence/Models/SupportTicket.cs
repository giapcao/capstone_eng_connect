using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("support_ticket")]
public partial class SupportTicket
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("created_by")]
    public Guid CreatedBy { get; set; }

    [Column("subject")]
    [StringLength(255)]
    public string Subject { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("type")]
    [StringLength(50)]
    public string? Type { get; set; }

    [Column("priority")]
    [StringLength(20)]
    public string? Priority { get; set; }

    [Column("status")]
    [StringLength(30)]
    public string Status { get; set; } = null!;

    [Column("closed_at")]
    public DateTime? ClosedAt { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("SupportTickets")]
    public virtual User CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Ticket")]
    public virtual ICollection<SupportTicketMessage> SupportTicketMessages { get; set; } = new List<SupportTicketMessage>();
}
