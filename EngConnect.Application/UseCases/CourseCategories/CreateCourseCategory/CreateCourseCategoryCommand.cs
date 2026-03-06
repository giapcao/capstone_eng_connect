using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseCategories.CreateCourseCategory;

public class CreateCourseCategoryCommand : ICommand
{
    public required Guid CourseId { get; set; }
    public required Guid CategoryId { get; set; }
}
