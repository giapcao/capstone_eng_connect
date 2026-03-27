using FluentValidation;

namespace EngConnect.Application.UseCases.CourseSessions.CreateCourseSession;

public class CreateCourseSessionCommandValidator : AbstractValidator<CreateCourseSessionCommand>
{
    public CreateCourseSessionCommandValidator()
    {
        RuleFor(x => x.CourseModuleId)
            .NotEmpty().WithMessage("CourseModuleId không được để trống");

        RuleFor(x => x.TutorId)
            .NotEmpty().WithMessage("TutorId không được để trống");

        RuleForEach(x => x.NewCourseSessions)
            .ChildRules(session =>
            {
                session.RuleFor(s => s.Title)
                    .MaximumLength(500).When(s => !string.IsNullOrEmpty(s.Title))
                    .WithMessage("Tiêu đề không được vượt quá 500 ký tự");

                session.RuleFor(s => s.SessionNumber)
                    .GreaterThan(0).When(s => s.SessionNumber.HasValue)
                    .WithMessage("Số thứ tự session phải lớn hơn 0");
            })
            .When(x => x.NewCourseSessions != null && x.NewCourseSessions.Count > 0);

        RuleForEach(x => x.CourseSessionIdExists)
            .ChildRules(session =>
            {
                session.RuleFor(s => s.CourseSessionId)
                    .NotEmpty().WithMessage("CourseSessionId không được để trống");

                session.RuleFor(s => s.SessionNumber)
                    .GreaterThan(0).When(s => s.SessionNumber.HasValue)
                    .WithMessage("Số thứ tự session phải lớn hơn 0");
            })
            .When(x => x.CourseSessionIdExists != null && x.CourseSessionIdExists.Count > 0);
    }
}
