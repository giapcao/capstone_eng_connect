namespace EngConnect.Application.UseCases.CourseCourseModules.Common;

public class GetCourseCourseModuleResponse
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public Guid CourseModuleId { get; set; }
    public int? ModuleNumber { get; set; }
}
