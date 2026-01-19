using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class CourseResource : AuditableEntity<Guid>
{
    public Guid SessionId { get; set; }

    public string? Title { get; set; }

    public string? ResourceType { get; set; }

    public string Url { get; set; } = null!;

    public string? Status { get; set; }

    public virtual CourseSession Session { get; set; } = null!;
}
