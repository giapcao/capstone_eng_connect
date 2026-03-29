using System;
using System.Collections.Generic;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class CourseSession : AuditableEntity<Guid>
{
    public Guid? TutorId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Outcomes { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    // 260318: Update relationship of course, courseModule, courseSession, courseResource from 1 - many to many - many
    public virtual ICollection<CourseModuleCourseSession> CourseModuleCourseSessions { get; set; } = new List<CourseModuleCourseSession>();
    public virtual ICollection<CourseSessionCourseResource> CourseSessionCourseResources { get; set; } = new List<CourseSessionCourseResource>();

    public virtual Tutor Tutor { get; set; } = null!;
}
