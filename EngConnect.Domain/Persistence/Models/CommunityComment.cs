using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class CommunityComment : AuditableEntity<Guid>
{
    public Guid PostId { get; set; }

    public Guid AuthorId { get; set; }

    public Guid? ParentCommentId { get; set; }

    public string Content { get; set; } = null!;
}
