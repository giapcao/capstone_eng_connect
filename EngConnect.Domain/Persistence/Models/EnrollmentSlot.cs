using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models
{
    public class EnrollmentSlot : AuditableEntity<Guid>
    {
        public Guid EnrollmentId { get; set; }

        public Guid TutorId { get; set; }

        public string? Weekday { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public virtual CourseEnrollment Enrollment { get; set; } = null!;

        public virtual Tutor Tutor { get; set; } = null!;
    }
}
