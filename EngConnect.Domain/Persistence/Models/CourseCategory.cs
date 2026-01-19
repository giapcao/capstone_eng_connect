using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class CourseCategory : AuditableEntity<Guid>
{
    public Guid CourseId { get; set; }

    public Guid CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;
}
