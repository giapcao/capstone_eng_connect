using FluentValidation;

namespace EngConnect.Application.UseCases.CourseSessions.CreateCourseSession;

public class CreateCourseSessionCommandValidator : AbstractValidator<CreateCourseSessionCommand>
{
    public CreateCourseSessionCommandValidator()
    {
        RuleFor(x => x.ModuleId)
            .NotEmpty().WithMessage("ModuleId không được để trống");
        
        RuleFor(x => x.Title)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Title))
            .WithMessage("Tiêu đề không được vượt quá 500 ký tự");
        
        RuleFor(x => x.SessionNumber)
            .GreaterThan(0).When(x => x.SessionNumber.HasValue)
            .WithMessage("Số thứ tự session phải lớn hơn 0");
    }
}
