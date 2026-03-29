using FluentValidation;

namespace EngConnect.Application.UseCases.CourseCourseModules.AddCourseModuleToCourse;

public class AddCourseModuleToCourseCommandValidator : AbstractValidator<AddCourseModuleToCourseCommand>
{
    public AddCourseModuleToCourseCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("CourseId không được để trống");

        RuleFor(x => x.CourseModuleId)
            .NotEmpty().WithMessage("CourseModuleId không được để trống");

        RuleFor(x => x.ModuleNumber)
            .GreaterThan(0).When(x => x.ModuleNumber.HasValue)
            .WithMessage("Số thứ tự module phải lớn hơn 0");
    }
}
