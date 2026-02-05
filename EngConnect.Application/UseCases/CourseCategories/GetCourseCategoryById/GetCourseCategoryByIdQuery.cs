using EngConnect.Application.UseCases.CourseCategories.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseCategories.GetCourseCategoryById;

public record GetCourseCategoryByIdQuery(Guid Id) : IQuery<GetCourseCategoryResponse>;
