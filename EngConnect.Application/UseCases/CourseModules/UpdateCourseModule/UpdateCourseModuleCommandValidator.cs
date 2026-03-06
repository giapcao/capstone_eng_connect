using FluentValidation;

namespace EngConnect.Application.UseCases.CourseModules.UpdateCourseModule;

public class UpdateCourseModuleCommandValidator : AbstractValidator<UpdateCourseModuleCommand>
{
    public UpdateCourseModuleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Tiêu đề không được để trống")
            .MaximumLength(500).WithMessage("Tiêu đề không được vượt quá 500 ký tự");
        
        RuleFor(x => x.ModuleNumber)
            .GreaterThan(0).When(x => x.ModuleNumber.HasValue)
            .WithMessage("Số thứ tự module phải lớn hơn 0");
    }
}
