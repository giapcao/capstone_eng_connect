using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class EmailTemplate : AuditableEntity<Guid>
{
    public string Name { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Body { get; set; } = null!;

    public string EventType { get; set; } = null!;

    public string Role { get; set; } = null!;

}
