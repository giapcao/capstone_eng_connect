using FluentValidation;

namespace EngConnect.Application.UseCases.Categories.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên danh mục không được để trống")
            .MaximumLength(200).WithMessage("Tên danh mục không được vượt quá 200 ký tự");
        
        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Mô tả không được vượt quá 1000 ký tự");
        
        RuleFor(x => x.Type)
            .MaximumLength(100).WithMessage("Loại danh mục không được vượt quá 100 ký tự");
    }
}
