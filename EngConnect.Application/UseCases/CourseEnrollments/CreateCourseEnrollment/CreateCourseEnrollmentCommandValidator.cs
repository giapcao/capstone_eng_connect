using FluentValidation;

namespace EngConnect.Application.UseCases.CourseEnrollments.CreateCourseEnrollment;

public class CreateCourseEnrollmentCommandValidator : AbstractValidator<CreateCourseEnrollmentCommand>
{
    public CreateCourseEnrollmentCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Mã khóa học không được để trống");
        
        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("Mã học sinh không được để trống");
        
        RuleFor(x => x.PriceAtPurchase)
            .GreaterThanOrEqualTo(0).WithMessage("Giá mua không được nhỏ hơn 0.")
            .When(x => x.PriceAtPurchase.HasValue);
        
        RuleFor(x => x.NumsOfSession)
            .GreaterThan(0).WithMessage("Số buổi học phải lớn hơn 0.")
            .When(x => x.NumsOfSession.HasValue);
        
        RuleFor(x => x.ExpiredAt)
            .NotEmpty().WithMessage("Ngày hết hạn không được để trống.")
            .GreaterThan(DateTime.Now).WithMessage("Ngày hết hạn phải là một thời điểm trong tương lai.")
            .When(x => x.ExpiredAt.HasValue);
    }
}
