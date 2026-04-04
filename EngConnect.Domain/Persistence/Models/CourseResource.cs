using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class CourseResource : AuditableEntity<Guid>
{
    public Guid? TutorId { get; set; }

    public string? Title { get; set; }

    public string? ResourceType { get; set; }

    public string Url { get; set; } = null!;

    public string? Status { get; set; }

    public virtual ICollection<CourseSessionCourseResource> CourseSessionCourseResources { get; set; } = new List<CourseSessionCourseResource>();
    
    public virtual Tutor Tutor { get; set; } = null!;
}
