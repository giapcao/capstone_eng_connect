using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class CourseReview : AuditableEntity<Guid>
{
    public Guid CourseId { get; set; }

    public Guid TutorId { get; set; }

    public Guid StudentId { get; set; }

    public Guid EnrollmentId { get; set; }

    public short? Rating { get; set; }

    public string? Comment { get; set; }

    public bool? IsAnonymous { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual CourseEnrollment Enrollment { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual Tutor Tutor { get; set; } = null!;
}
