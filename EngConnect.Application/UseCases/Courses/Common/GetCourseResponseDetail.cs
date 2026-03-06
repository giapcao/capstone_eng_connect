namespace EngConnect.Application.UseCases.Courses.Common;

public class GetCourseResponseDetail: GetCourseResponse
{
    public List<GetCourseCategoryResponseInCourse>? CourseCategories { get; set; }
    public List<GetCourseModuleResponseInCourse>? CourseModules { get; set; }
}

public class GetCourseCategoryResponseInCourse
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? CategoryDescription { get; set; }
    public string? CategoryType { get; set; }
}

public class GetCourseModuleResponseInCourse
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string ModuleTitle { get; set; } = null!;
    public string? ModuleDescription { get; set; }
    public string? ModuleOutcomes { get; set; }
    public int? ModuleNumber { get; set; }
    public List<GetSessonResponseInCourseModule>? CourseSessions { get; set; }
}

public class GetSessonResponseInCourseModule
{
    public Guid Id { get; set; }
    public Guid ModuleId { get; set; }
    public string? SessionTitle { get; set; }
    public string? SessionDescription { get; set; }
    public string? SessionOutcomes { get; set; }
    public int? SessionNumber { get; set; }
}