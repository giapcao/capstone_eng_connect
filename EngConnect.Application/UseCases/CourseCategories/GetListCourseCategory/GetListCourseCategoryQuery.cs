using EngConnect.Application.UseCases.CourseCategories.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.CourseCategories.GetListCourseCategory;

public record GetListCourseCategoryQuery : BaseQuery<PaginationResult<GetCourseCategoryResponse>>
{
    public Guid? CourseId { get; set; }
    public Guid? CategoryId { get; set; }
}
