using FluentValidation;

namespace EngConnect.Application.UseCases.CourseSessions.CreateCourseSession;

public class CreateCourseSessionCommandValidator : AbstractValidator<CreateCourseSessionCommand>
{
    public CreateCourseSessionCommandValidator()
    {
        RuleFor(x => x.CourseModuleId)
            .NotEmpty().WithMessage("CourseModuleId khong duoc de trong");

        RuleFor(x => x)
            .Must(x => (x.NewCourseSessions?.Count ?? 0) > 0 || (x.CourseSessionIdExists?.Count ?? 0) > 0)
            .WithMessage("Phai co it nhat 1 session de them vao module");

        RuleForEach(x => x.NewCourseSessions)
            .ChildRules(session =>
            {
                session.RuleFor(s => s.Title)
                    .MaximumLength(500).When(s => !string.IsNullOrEmpty(s.Title))
                    .WithMessage("Tieu de khong duoc vuot qua 500 ky tu");

                session.RuleFor(s => s.SessionNumber)
                    .GreaterThan(0).When(s => s.SessionNumber.HasValue)
                    .WithMessage("So thu tu session phai lon hon 0");
            })
            .When(x => x.NewCourseSessions != null && x.NewCourseSessions.Count > 0);

        RuleForEach(x => x.CourseSessionIdExists)
            .ChildRules(session =>
            {
                session.RuleFor(s => s.CourseSessionId)
                    .NotEmpty().WithMessage("CourseSessionId khong duoc de trong");

                session.RuleFor(s => s.SessionNumber)
                    .GreaterThan(0).When(s => s.SessionNumber.HasValue)
                    .WithMessage("So thu tu session phai lon hon 0");
            })
            .When(x => x.CourseSessionIdExists != null && x.CourseSessionIdExists.Count > 0);
    }
}
