namespace EngConnect.Application.UseCases.CourseCategories.Common;

public class GetCourseCategoryResponse
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public Guid CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
