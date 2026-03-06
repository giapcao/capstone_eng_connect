using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseSessions.CreateCourseSession;

public class CreateCourseSessionCommand : ICommand
{
    public required Guid ModuleId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Outcomes { get; set; }
    public int? SessionNumber { get; set; }
}
