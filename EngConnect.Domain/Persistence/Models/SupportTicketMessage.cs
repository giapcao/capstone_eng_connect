using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class SupportTicketMessage : AuditableEntity<Guid>
{
    public Guid TicketId { get; set; }

    public Guid SenderId { get; set; }

    public string Message { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;

    public virtual SupportTicket Ticket { get; set; } = null!;
}
