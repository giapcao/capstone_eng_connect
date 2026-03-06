using FluentValidation;

namespace EngConnect.Application.UseCases.CourseCategories.UpdateCourseCategory;

public class UpdateCourseCategoryCommandValidator : AbstractValidator<UpdateCourseCategoryCommand>
{
    public UpdateCourseCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống");
        
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId không được để trống");
    }
}
