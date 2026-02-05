using FluentValidation;

namespace EngConnect.Application.UseCases.CourseVerificationRequests.CreateCourseVerificationRequest;

public class CreateCourseVerificationRequestCommandValidator : AbstractValidator<CreateCourseVerificationRequestCommand>
{
    public CreateCourseVerificationRequestCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("CourseId không được để trống");
    }
}
