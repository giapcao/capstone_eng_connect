using System.Text.Json.Serialization;
using EngConnect.Application.UseCases.CourseSessions.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseSessions.CreateCourseSession;

public class CreateCourseSessionCommand : ICommand<GetCourseSessionListResponse>
{
    [JsonIgnore]
    public Guid? TutorId { get; set; }
    public required Guid CourseModuleId { get; set; }

    public List<AddNewCourseSession>? NewCourseSessions { get; set; } = [];

    public List<CourseSessionIdExist>? CourseSessionIdExists { get; set; } = [];
}

public class CourseSessionIdExist
{
    public Guid CourseSessionId { get; set; }
    public int? SessionNumber { get; set; }
}

public class AddNewCourseSession
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Outcomes { get; set; }
    public int? SessionNumber { get; set; }
}
