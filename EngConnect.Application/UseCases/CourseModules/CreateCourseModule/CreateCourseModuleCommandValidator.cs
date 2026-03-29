using FluentValidation;

namespace EngConnect.Application.UseCases.CourseModules.CreateCourseModule;

public class CreateCourseModuleCommandValidator : AbstractValidator<CreateCourseModuleCommand>
{
    public CreateCourseModuleCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("CourseId khong duoc de trong");

        RuleFor(x => x)
            .Must(x => (x.NewCourseModules?.Count ?? 0) > 0 || (x.CourseModuleIdExists?.Count ?? 0) > 0)
            .WithMessage("Phai co it nhat 1 module de them vao khoa hoc");

        RuleForEach(x => x.NewCourseModules)
            .ChildRules(module =>
            {
                module.RuleFor(m => m.Title)
                    .NotEmpty().WithMessage("Tieu de khong duoc de trong")
                    .MaximumLength(500).WithMessage("Tieu de khong duoc vuot qua 500 ky tu");

                module.RuleFor(m => m.ModuleNumber)
                    .GreaterThan(0).When(m => m.ModuleNumber.HasValue)
                    .WithMessage("So thu tu module phai lon hon 0");
            })
            .When(x => x.NewCourseModules != null && x.NewCourseModules.Count > 0);

        RuleForEach(x => x.CourseModuleIdExists)
            .ChildRules(module =>
            {
                module.RuleFor(m => m.CourseModuleId)
                    .NotEmpty().WithMessage("CourseModuleId khong duoc de trong");

                module.RuleFor(m => m.ModuleNumber)
                    .GreaterThan(0).When(m => m.ModuleNumber.HasValue)
                    .WithMessage("So thu tu module phai lon hon 0");
            })
            .When(x => x.CourseModuleIdExists != null && x.CourseModuleIdExists.Count > 0);
    }
}
