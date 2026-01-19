using System;
using System.Collections.Generic;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class CourseModule : AuditableEntity<Guid>
{
    public Guid CourseId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Outcomes { get; set; }

    public int? ModuleNumber { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<CourseSession> CourseSessions { get; set; } = new List<CourseSession>();
}
