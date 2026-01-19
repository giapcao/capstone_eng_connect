using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class ConversationMessage : AuditableEntity<Guid>
{
    public Guid ConversationId { get; set; }

    public Guid SenderId { get; set; }

    public string Message { get; set; } = null!;

    public virtual Conversation Conversation { get; set; } = null!;
}
