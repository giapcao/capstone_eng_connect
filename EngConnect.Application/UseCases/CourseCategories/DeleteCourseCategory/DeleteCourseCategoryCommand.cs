using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseCategories.DeleteCourseCategory;

public record DeleteCourseCategoryCommand(Guid Id) : ICommand;
