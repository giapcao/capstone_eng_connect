using EngConnect.BuildingBlock.Application.Validation.Validation;
using EngConnect.Domain.Constants;
using FluentValidation;

namespace EngConnect.Application.UseCases.Lessons.UpdateLesson;

public class UpdateLessonCommandValidator : AbstractValidator<UpdateLessonCommand>
{
    public UpdateLessonCommandValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("Mã học sinh không được để trống");
        
        RuleFor(x => x.EnrollmentId)
            .NotEmpty().WithMessage("Mã enrollment không được để trống");
        
        RuleFor(x => x.StartTime)
            .GreaterThan(DateTime.UtcNow).WithMessage("Thời gian bắt đầu phải ở trong tương lai.");
        
        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu.");

    }
}
