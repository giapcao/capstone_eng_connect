using EngConnect.Application.UseCases.CourseResources.CreateCourseResource;
using Microsoft.AspNetCore.Http;

namespace EngConnect.Application.UseCases.CourseResources.Common;

public class CreateCourseResourceRequest
{
    public Guid CourseSessionId { get; set; }
    public string Title { get; set; } = null!;
    public string ResourceType { get; set; }  = null!;
    public IFormFile ResourceFile { get; set; } = null!;
    public string? ResourceFileName { get; set; }
}