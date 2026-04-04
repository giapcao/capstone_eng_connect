using FluentValidation;

namespace EngConnect.Application.UseCases.CourseModuleCourseSessions.AddCourseSessionToCourseModule;

public class AddCourseSessionToCourseModuleCommandValidator : AbstractValidator<AddCourseSessionToCourseModuleCommand>
{
    public AddCourseSessionToCourseModuleCommandValidator()
    {
        RuleFor(x => x.CourseModuleId)
            .NotEmpty().WithMessage("CourseModuleId không được để trống");

        RuleFor(x => x.CourseSessionId)
            .NotEmpty().WithMessage("CourseSessionId không được để trống");

        RuleFor(x => x.SessionNumber)
            .GreaterThan(0).When(x => x.SessionNumber.HasValue)
            .WithMessage("Số thứ tự buổi học phải lớn hơn 0");
    }
}
