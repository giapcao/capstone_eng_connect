using System.Text.Json.Serialization;
using EngConnect.Application.UseCases.CourseResources.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.Files;

namespace EngConnect.Application.UseCases.CourseResources.CreateCourseResource;

public class CreateCourseResourceCommand : ICommand<GetCourseResourceResponse>
{
    public Guid? TutorId { get; set; }
    public Guid CourseSessionId { get; set; }
    public string Title { get; set; } = null!;
    public string ResourceType { get; set; } = null!;
    public FileUpload ResourceFile { get; set; } = null!;
}
