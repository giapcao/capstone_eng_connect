namespace EngConnect.Application.UseCases.CourseSessions.Common;

public class GetCourseSessionResponse
{
    public Guid Id { get; set; }
    public Guid ModuleId { get; set; }
    public Guid? ParentSessionId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Outcomes { get; set; }
    public int? SessionNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class GetCourseSessionListResponse
{
    public Guid ModuleId { get; set; }
    public List<GetCourseSessionResponse> CourseSessions { get; set; } = [];
}

public class GetCourseSessionTreeNodeResponse
{
    public Guid Id { get; set; }
    public Guid? ParentSessionId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Outcomes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class GetCourseSessionTreeResponse
{
    public GetCourseSessionTreeNodeResponse Session { get; set; } = null!;
    public List<GetCourseSessionTreeNodeResponse> ParentChain { get; set; } = [];
}
