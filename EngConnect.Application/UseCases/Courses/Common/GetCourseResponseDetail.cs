namespace EngConnect.Application.UseCases.Courses.Common;

public class GetCourseResponseDetail: GetCourseResponse
{
    public List<GetCourseModuleResponseInCourse> CourseCourseModules { get; set; } = [];
}

public class GetCourseCategoryResponseInCourse
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? CategoryDescription { get; set; }
    public string? CategoryType { get; set; }
}

public class GetCourseModuleResponseInCourse
{
    public Guid Id { get; set; }
    public Guid CourseModuleId {get; set;}
    public Guid? ParentModuleId { get; set; }
    public string ModuleTitle { get; set; } = null!;
    public string? ModuleDescription { get; set; }
    public string? ModuleOutcomes { get; set; }
    public int? ModuleNumber { get; set; }
    public List<GetSessonResponseInCourseModule> CourseModuleCourseSessions { get; set; } = [];
}

public class GetSessonResponseInCourseModule
{
    public Guid Id { get; set; }
    public Guid CourseSessionId { get; set; }
    public Guid? ParentSessionId { get; set; }
    public string? SessionTitle { get; set; }
    public string? SessionDescription { get; set; }
    public string? SessionOutcomes { get; set; }
    public int? SessionNumber { get; set; }
}
