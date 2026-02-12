using System;
using System.Collections.Generic;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;
using EngConnect.Domain.Constants;

namespace EngConnect.Domain.Persistence.Models;

public class Tutor : AuditableEntity<Guid>
{
    public Guid UserId { get; set; }

    public string? Headline { get; set; }

    public string? Bio { get; set; }

    public string? IntroVideoUrl { get; set; }

    public int? YearsExperience { get; set; }

    public string? CvUrl { get; set; }

    public List<string>? Tags { get; set; }

    public int? SlotsCount { get; set; }

    public string? Status { get; set; }

    public string? VerifiedStatus { get; set; }

    public decimal? RatingAverage { get; set; }

    public int? RatingCount { get; set; }

    public virtual ICollection<ActualSchedule> ActualSchedules { get; set; } = new List<ActualSchedule>();

    public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

    public virtual ICollection<CourseReview> CourseReviews { get; set; } = new List<CourseReview>();

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual ICollection<TutorDocument> TutorDocuments { get; set; } = new List<TutorDocument>();

    public virtual ICollection<TutorSchedule> TutorSchedules { get; set; } = new List<TutorSchedule>();

    public virtual ICollection<TutorVerificationRequest> TutorVerificationRequests { get; set; } = new List<TutorVerificationRequest>();

    public virtual User User { get; set; } = null!;
    
    public static Tutor CreateTutorWithUserId(Guid userId)
    {
        return new Tutor
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Status = nameof(TutorStatus.Active),
            VerifiedStatus = nameof(TutorVerifiedStatus.Unverified),
            CreatedAt = DateTime.UtcNow
        };
    }
}
