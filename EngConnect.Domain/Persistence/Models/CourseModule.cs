using System;
using System.Collections.Generic;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class CourseModule : AuditableEntity<Guid>
{
    public Guid? TutorId { get; set; }
    public Guid? ParentModuleId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Outcomes { get; set; }
    
    // 260318: Update relationship of course, courseModule, courseSession, courseResource from 1 - many to many - many
    public virtual ICollection<CourseCourseModule> CourseCourseModules { get; set; } = new List<CourseCourseModule>();
    public virtual ICollection<CourseModuleCourseSession> CourseModuleCourseSessions { get; set; } = new List<CourseModuleCourseSession>();
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    public virtual ICollection<CourseModule> InverseParentModule { get; set; } = new List<CourseModule>();
    public virtual CourseModule? ParentModule { get; set; }
    public virtual Tutor Tutor { get; set; } = null!;
    
}
