using EngConnect.Application.UseCases.Courses.Common;
using EngConnect.Application.UseCases.CourseSessions.Common;

namespace EngConnect.Application.UseCases.CourseModules.Common;

public class GetCourseModuleResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Outcomes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class GetCourseModuleDetailResponse : GetCourseModuleResponse
{
    public List<GetSessonResponseInCourseModule> CourseSessions { get; set; } = [];
}
