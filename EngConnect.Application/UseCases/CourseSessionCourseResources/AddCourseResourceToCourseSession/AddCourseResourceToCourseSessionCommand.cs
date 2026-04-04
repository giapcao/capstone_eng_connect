using EngConnect.BuildingBlock.Application.Base;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EngConnect.Application.UseCases.CourseSessionCourseResources.AddCourseResourceToCourseSession;

public class AddCourseResourceToCourseSessionCommand : ICommand
{
    public Guid CourseSessionId { get; set; }

    public List<AddCourseResourceExist> CourseResources { get; set; } = [];
}

public abstract class AddCourseResourceExist
{
    public Guid CourseResourceId { get; set; }
}
