using EngConnect.Application.UseCases.Courses.Common;

namespace EngConnect.Application.UseCases.CourseModules.Common;

public class GetCourseModuleResponse
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Outcomes { get; set; }
    public int? ModuleNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class GetCourseModuleListResponse
{
    public Guid CourseId { get; set; }
    public List<GetCourseModuleResponse> CourseModules { get; set; } = [];
}

public class GetCourseModuleDetailResponse : GetCourseModuleResponse
{
    public List<GetSessonResponseInCourseModule> CourseSessions { get; set; } = [];
}
