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

    public int? MonthExperience { get; set; }

    public string? CvUrl { get; set; }

    public string? Avatar { get; set; }

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
    
    // 260318: Update relationship of course, courseModule, courseSession, courseResource from 1 - many to many - many
    public virtual ICollection<CourseModule> CourseModules { get; set; } = new List<CourseModule>();
    public virtual ICollection<CourseSession> CourseSessions { get; set; } = new List<CourseSession>();
    public virtual ICollection<CourseResource> CourseResources { get; set; } = new List<CourseResource>();


    public virtual ICollection<TutorDocument> TutorDocuments { get; set; } = new List<TutorDocument>();

    public virtual ICollection<TutorSchedule> TutorSchedules { get; set; } = new List<TutorSchedule>();

    public virtual ICollection<EnrollmentSlot> EnrollmentSlots { get; set; } = new List<EnrollmentSlot>();

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

    public static Tutor CreateTutorWithUserId(
        Guid userId,
        string? headline,
        string? bio,
        string? introVideoUrl,
        int? monthExperience,
        string? cvUrl)
    {
        return new Tutor
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Headline = headline,
            Bio = bio,
            IntroVideoUrl = introVideoUrl,
            MonthExperience = monthExperience,
            CvUrl = cvUrl,
            SlotsCount = 0,
            RatingAverage = 0,
            RatingCount = 0,
            Status = nameof(TutorStatus.Active),
            VerifiedStatus = nameof(TutorVerifiedStatus.Unverified),
            CreatedAt = DateTime.UtcNow
        };
    }
}
