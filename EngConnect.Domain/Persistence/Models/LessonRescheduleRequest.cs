using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models
{
    public class LessonRescheduleRequest : AuditableEntity<Guid>
    {
        public Guid LessonId { get; set; }

        public Guid StudentId { get; set; }

        public DateTime ProposedStartTime { get; set; }

        public DateTime ProposedEndTime { get; set; }

        public string? Status { get; set; }

        public string? TutorNote { get; set; }

        public virtual Lesson Lesson { get; set; } = null!;

        public virtual Student Student { get; set; } = null!;
    }
}
