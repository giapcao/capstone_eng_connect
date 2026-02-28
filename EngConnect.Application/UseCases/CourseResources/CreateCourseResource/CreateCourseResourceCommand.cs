using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseResources.CreateCourseResource;

public class CreateCourseResourceCommand : ICommand
{
    public required Guid SessionId { get; set; }
    public string? Title { get; set; }
    public string? ResourceType { get; set; }
    public required string Url { get; set; }
}
