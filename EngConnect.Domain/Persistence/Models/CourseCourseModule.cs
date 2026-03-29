using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class CourseCourseModule: AuditableEntity<Guid>
{
    public Guid CourseId { get; set; }
    public Guid CourseModuleId { get; set; }
    public int? ModuleNumber { get; set; }
    public virtual Course Course { get; set; } = null!;
    public virtual CourseModule CourseModule { get; set; } = null!;
}