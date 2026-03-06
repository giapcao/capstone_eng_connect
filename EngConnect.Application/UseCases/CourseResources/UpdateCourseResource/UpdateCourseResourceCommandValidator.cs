using FluentValidation;

namespace EngConnect.Application.UseCases.CourseResources.UpdateCourseResource;

public class UpdateCourseResourceCommandValidator : AbstractValidator<UpdateCourseResourceCommand>
{
    public UpdateCourseResourceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống");
        
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Url không được để trống")
            .MaximumLength(1000).WithMessage("Url không được vượt quá 1000 ký tự");
        
        RuleFor(x => x.Title)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Title))
            .WithMessage("Tiêu đề không được vượt quá 500 ký tự");
    }
}
