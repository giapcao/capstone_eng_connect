using System.Text.Json.Serialization;
using EngConnect.Application.UseCases.CourseSessions.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseSessions.UpdateCourseSession;

public class UpdateCourseSessionCommand : ICommand<GetCourseSessionResponse>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Outcomes { get; set; }
}
