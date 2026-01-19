using System;
using System.Collections.Generic;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class Conversation : AuditableEntity<Guid>
{
    public Guid TutorId { get; set; }

    public Guid StudentId { get; set; }

    public virtual ICollection<ConversationMessage> ConversationMessages { get; set; } = new List<ConversationMessage>();

    public virtual Student Student { get; set; } = null!;

    public virtual Tutor Tutor { get; set; } = null!;
}
