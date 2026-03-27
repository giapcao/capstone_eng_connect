namespace EngConnect.Application.UseCases.CourseModuleCourseSessions.Common;

public class GetCourseModuleCourseSessionResponse
{
    public Guid Id { get; set; }
    public Guid CourseModuleId { get; set; }
    public Guid CourseSessionId { get; set; }
    public int? SessionNumber { get; set; }
}
