using EngConnect.BuildingBlock.Application.Base;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngConnect.Application.UseCases.CourseCourseModules.AddCourseModuleToCourse;

public class AddCourseModuleToCourseCommand : ICommand
{
    [BindNever]
    public Guid CourseId { get; set; }
    
    public required Guid CourseModuleId { get; set; }
    
    public int? ModuleNumber { get; set; }
}
