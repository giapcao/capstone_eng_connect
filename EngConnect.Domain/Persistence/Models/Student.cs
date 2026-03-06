using System;
using System.Collections.Generic;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;
using EngConnect.Domain.Constants;

namespace EngConnect.Domain.Persistence.Models;

public class Student : AuditableEntity<Guid>
{
    public Guid UserId { get; set; }

    public string? Notes { get; set; }

    public string? School { get; set; }

    public string? Grade { get; set; }

    public string? Class { get; set; }

    public List<string>? Tags { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<ActualSchedule> ActualSchedules { get; set; } = new List<ActualSchedule>();

    public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

    public virtual ICollection<CourseEnrollment> CourseEnrollments { get; set; } = new List<CourseEnrollment>();

    public virtual ICollection<CourseReview> CourseReviews { get; set; } = new List<CourseReview>();

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual ICollection<LessonRescheduleRequest> LessonRescheduleRequests { get; set; } = new List<LessonRescheduleRequest>();

    public virtual User User { get; set; } = null!;
    
    public static Student CreateStudentWithUserId(Guid userId)
    {
        return new Student
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Status = nameof(CommonStatus.Active),
            CreatedAt = DateTime.UtcNow,
        };
    }
    
    public static Student CreateStudentWithUserId(Guid userId, string? school, string? grade, string? @class)
    {
        return new Student
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            School = school,
            Grade = grade,
            Class = @class,
            Status = nameof(CommonStatus.Active),
            CreatedAt = DateTime.UtcNow,
        };
    }
}


