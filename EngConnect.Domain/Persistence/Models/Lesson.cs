using System;
using System.Collections.Generic;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class Lesson : AuditableEntity<Guid>
{
    public Guid TutorId { get; set; }
    public Guid StudentId { get; set; }

    public Guid EnrollmentId { get; set; }

    public Guid? SessionId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? Status { get; set; }

    public string? MeetingUrl { get; set; }

    public string? MeetingStatus { get; set; }

    public DateTime? MeetingStartedAt { get; set; }

    public DateTime? MeetingEndedAt { get; set; }

    public virtual CourseEnrollment Enrollment { get; set; } = null!;

    public virtual ICollection<LessonHomework> LessonHomeworks { get; set; } = new List<LessonHomework>();

    public virtual ICollection<LessonRecord> LessonRecords { get; set; } = new List<LessonRecord>();

    public virtual ICollection<LessonScript> LessonScripts { get; set; } = new List<LessonScript>();

    public virtual ICollection<LessonRescheduleRequest> LessonRescheduleRequests { get; set; } = new List<LessonRescheduleRequest>();
    
    public virtual ICollection<MeetingParticipant> MeetingParticipants { get; set; } = new List<MeetingParticipant>();

    public virtual CourseSession? Session { get; set; }

    public virtual Student Student { get; set; } = null!;
    
     public virtual Tutor Tutor { get; set; } = null!;
}
