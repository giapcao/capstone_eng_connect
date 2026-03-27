using EngConnect.BuildingBlock.Application.Base;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngConnect.Application.UseCases.CourseModuleCourseSessions.AddCourseSessionToCourseModule;

public class AddCourseSessionToCourseModuleCommand : ICommand
{
    public Guid CourseModuleId { get; set; }
    
    public required Guid CourseSessionId { get; set; }
    
    public int? SessionNumber { get; set; }
}
