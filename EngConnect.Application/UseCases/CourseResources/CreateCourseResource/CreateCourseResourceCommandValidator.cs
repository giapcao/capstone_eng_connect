using FluentValidation;

namespace EngConnect.Application.UseCases.CourseResources.CreateCourseResource;

public class CreateCourseResourceCommandValidator : AbstractValidator<CreateCourseResourceCommand>
{
    public CreateCourseResourceCommandValidator()
    {
        RuleFor(x => x.SessionId)
            .NotEmpty().WithMessage("SessionId không được để trống");
        
        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Url không được để trống")
            .MaximumLength(1000).WithMessage("Url không được vượt quá 1000 ký tự");
        
        RuleFor(x => x.Title)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Title))
            .WithMessage("Tiêu đề không được vượt quá 500 ký tự");
    }
}
