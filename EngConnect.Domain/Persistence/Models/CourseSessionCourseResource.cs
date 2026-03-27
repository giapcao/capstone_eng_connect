using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class CourseSessionCourseResource: AuditableEntity<Guid>
{
    public Guid CourseSessionId { get; set; }
    public Guid CourseResourceId { get; set; }
    public virtual CourseSession CourseSession { get; set; } = null!;
    public virtual CourseResource CourseResource { get; set; } = null!;
}