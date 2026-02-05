using FluentValidation;

namespace EngConnect.Application.UseCases.CourseVerificationRequests.UpdateCourseVerificationRequest;

public class UpdateCourseVerificationRequestCommandValidator : AbstractValidator<UpdateCourseVerificationRequestCommand>
{
    public UpdateCourseVerificationRequestCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống");
    }
}
