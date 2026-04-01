using FluentValidation;

namespace EngConnect.Application.UseCases.Lessons.UpdateLesson;

public class UpdateLessonCommandValidator : AbstractValidator<UpdateLessonCommand>
{
    public UpdateLessonCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống");

        RuleFor(x => x.TutorId)
            .NotEmpty().WithMessage("Mã gia sư không được để trống");

        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("Mã học sinh không được để trống");

        RuleFor(x => x.EnrollmentId)
            .NotEmpty().WithMessage("Mã enrollment không được để trống");

        RuleFor(x => x.StartTime)
            .NotNull().WithMessage("Thời gian bắt đầu không được để trống.")
            .GreaterThan(DateTime.UtcNow).WithMessage("Thời gian bắt đầu phải ở trong tương lai.");

        RuleFor(x => x.EndTime)
            .NotNull().WithMessage("Thời gian kết thúc không được để trống.")
            .GreaterThan(x => x.StartTime).WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu.");
    }
}
