using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class CommunityPost : AuditableEntity<Guid>
{
    public Guid AuthorId { get; set; }

    public string Title { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Content { get; set; } = null!;

    public bool? IsPinned { get; set; }
}
