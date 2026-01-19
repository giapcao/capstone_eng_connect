using System;
using System.Collections.Generic;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class Category : AuditableEntity<Guid>
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<CourseCategory> CourseCategories { get; set; } = new List<CourseCategory>();
}
