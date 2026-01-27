using EngConnect.BuildingBlock.Application.Validation.Validation;
using FluentValidation;

namespace EngConnect.Application.UseCases.Lessons.CreateLesson;

public class CreateLessonCommandValidator : AbstractValidator<CreateLessonCommand>
{
    public CreateLessonCommandValidator()
    {
        RuleFor(x => x.TutorId)
            .NotEmpty().WithMessage("Mã gia sư (TutorId) không được để trống.");

        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("Mã học sinh (StudentId) không được để trống.");

        RuleFor(x => x.EnrollmentId)
            .NotEmpty().WithMessage("Mã đăng ký (EnrollmentId) không được để trống.");
        
        RuleFor(x => x.StartTime)
            .GreaterThan(DateTime.UtcNow).WithMessage("Thời gian bắt đầu phải ở trong tương lai.");
        
        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu.");
    }
}
