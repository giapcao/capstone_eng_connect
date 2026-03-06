using System.Text.Json.Serialization;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseCategories.UpdateCourseCategory;

public class UpdateCourseCategoryCommand : ICommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public required Guid CategoryId { get; set; }
}
