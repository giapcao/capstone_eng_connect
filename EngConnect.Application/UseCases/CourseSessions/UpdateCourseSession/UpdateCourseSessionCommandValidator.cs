using FluentValidation;

namespace EngConnect.Application.UseCases.CourseSessions.UpdateCourseSession;

public class UpdateCourseSessionCommandValidator : AbstractValidator<UpdateCourseSessionCommand>
{
    public UpdateCourseSessionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống");
        
        RuleFor(x => x.Title)
            .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Title))
            .WithMessage("Tiêu đề không được vượt quá 500 ký tự");
    }
}
