using FluentValidation;

namespace EngConnect.Application.UseCases.CourseSessionCourseResources.AddCourseResourceToCourseSession;

public class AddCourseResourceToCourseSessionCommandValidator : AbstractValidator<AddCourseResourceToCourseSessionCommand>
{
    public AddCourseResourceToCourseSessionCommandValidator()
    {
        RuleFor(x => x.CourseSessionId)
            .NotEmpty().WithMessage("CourseSessionId không được để trống");
    }
}
