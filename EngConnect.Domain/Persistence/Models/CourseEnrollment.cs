using System;
using System.Collections.Generic;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class CourseEnrollment : AuditableEntity<Guid>
{
    public Guid CourseId { get; set; }

    public Guid StudentId { get; set; }

    public decimal? PriceAtPurchase { get; set; }

    public string? Currency { get; set; }

    public int? NumsOfSession { get; set; }

    public string? Status { get; set; }

    public DateTime? EnrolledAt { get; set; }

    public DateTime? ExpiredAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual CourseReview? CourseReview { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual ICollection<EnrollmentSlot> EnrollmentSlots { get; set; } = new List<EnrollmentSlot>();

    public virtual Student Student { get; set; } = null!;
}
