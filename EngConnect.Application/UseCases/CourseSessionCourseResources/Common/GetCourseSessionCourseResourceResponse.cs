namespace EngConnect.Application.UseCases.CourseSessionCourseResources.Common;

public class GetCourseSessionCourseResourceResponse
{
    public Guid Id { get; set; }
    public Guid CourseSessionId { get; set; }
    public Guid CourseResourceId { get; set; }
}
