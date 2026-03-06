using System;
using System.Collections.Generic;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class Course : AuditableEntity<Guid>
{
    public Guid TutorId { get; set; }

    public Guid? ParentCourseId { get; set; }

    public string Title { get; set; } = null!;

    public string? ShortDescription { get; set; }

    public string? FullDescription { get; set; }

    public string? Outcomes { get; set; }

    public string? Level { get; set; }

    public TimeSpan? EstimatedTime { get; set; }

    public TimeSpan? EstimatedTimeLesson { get; set; }

    public decimal Price { get; set; }

    /// <summary>
    /// Currency code for the price
    /// </summary>
    public string? Currency { get; set; }

    public int? NumberOfSessions { get; set; }

    public int? NumsSessionInWeek { get; set; }

    public string? ThumbnailUrl { get; set; }

    public string? DemoVideoUrl { get; set; }

    public int NumberOfEnrollment { get; set; } = 0;

    public decimal? RatingAverage { get; set; }

    public int? RatingCount { get; set; }

    public string? Status { get; set; }

    public bool IsCertificate { get; set; } = false;

    public virtual ICollection<ActualSchedule> ActualSchedules { get; set; } = new List<ActualSchedule>();

    public virtual ICollection<CourseCategory> CourseCategories { get; set; } = new List<CourseCategory>();

    public virtual ICollection<CourseEnrollment> CourseEnrollments { get; set; } = new List<CourseEnrollment>();

    public virtual ICollection<CourseModule> CourseModules { get; set; } = new List<CourseModule>();

    public virtual ICollection<CourseReview> CourseReviews { get; set; } = new List<CourseReview>();

    public virtual ICollection<CourseVerificationRequest> CourseVerificationRequests { get; set; } = new List<CourseVerificationRequest>();

    public virtual ICollection<Course> InverseParentCourse { get; set; } = new List<Course>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Course? ParentCourse { get; set; }

    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    public virtual Tutor Tutor { get; set; } = null!;
}
