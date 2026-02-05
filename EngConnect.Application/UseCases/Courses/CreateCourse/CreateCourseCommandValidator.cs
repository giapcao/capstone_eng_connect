using FluentValidation;

namespace EngConnect.Application.UseCases.Courses.CreateCourse;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.TutorId)
            .NotEmpty().WithMessage("TutorId không được để trống");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Tiêu đề khóa học không được để trống")
            .MaximumLength(500).WithMessage("Tiêu đề không được vượt quá 500 ký tự");
        
        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Giá phải lớn hơn hoặc bằng 0");
        
        RuleFor(x => x.NumberOfSessions)
            .GreaterThan(0)
            .WithMessage("Số buổi học phải lớn hơn 0");
        
        RuleFor(x => x.NumsSessionInWeek)
            .GreaterThan(0)
            .WithMessage("Số buổi học trong tuần phải lớn hơn 0");
    }
}
