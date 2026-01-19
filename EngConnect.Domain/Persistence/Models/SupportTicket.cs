using System;
using System.Collections.Generic;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class SupportTicket : AuditableEntity<Guid>
{
    public Guid CreatedBy { get; set; }

    public string Subject { get; set; } = null!;

    public string? Description { get; set; }

    public string? Type { get; set; }

    public string? Priority { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? ClosedAt { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<SupportTicketMessage> SupportTicketMessages { get; set; } = new List<SupportTicketMessage>();
}
