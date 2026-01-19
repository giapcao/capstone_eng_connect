using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class EmailTemplate : AuditableEntity<Guid>
{
    public string Name { get; set; } = null!;

    public string SubjectTemplate { get; set; } = null!;

    public string? BodyHtmlTemplate { get; set; }

    public string? BodyTextTemplate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
