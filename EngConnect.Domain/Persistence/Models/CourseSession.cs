using System;
using System.Collections.Generic;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class CourseSession : AuditableEntity<Guid>
{
    public Guid ModuleId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Outcomes { get; set; }

    public int? SessionNumber { get; set; }

    public virtual ICollection<CourseResource> CourseResources { get; set; } = new List<CourseResource>();

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual CourseModule Module { get; set; } = null!;
}
