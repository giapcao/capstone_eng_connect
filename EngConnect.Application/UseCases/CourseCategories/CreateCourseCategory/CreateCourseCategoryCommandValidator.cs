using FluentValidation;

namespace EngConnect.Application.UseCases.CourseCategories.CreateCourseCategory;

public class CreateCourseCategoryCommandValidator : AbstractValidator<CreateCourseCategoryCommand>
{
    public CreateCourseCategoryCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("CourseId không được để trống");
        
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId không được để trống");
    }
}
