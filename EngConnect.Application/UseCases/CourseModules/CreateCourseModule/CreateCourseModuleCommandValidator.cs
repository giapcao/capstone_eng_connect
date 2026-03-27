using FluentValidation;

namespace EngConnect.Application.UseCases.CourseModules.CreateCourseModule;

public class CreateCourseModuleCommandValidator : AbstractValidator<CreateCourseModuleCommand>
{
    public CreateCourseModuleCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("CourseId không được để trống");
    }
}
