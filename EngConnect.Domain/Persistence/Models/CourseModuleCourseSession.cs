using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class CourseModuleCourseSession: AuditableEntity<Guid>
{
    public Guid CourseModuleId { get; set; }
    public Guid CourseSessionId { get; set; }
    public int? SessionNumber { get; set; }
    public virtual CourseModule CourseModule { get; set; } = null!;
    public virtual CourseSession CourseSession { get; set; } = null!;
}