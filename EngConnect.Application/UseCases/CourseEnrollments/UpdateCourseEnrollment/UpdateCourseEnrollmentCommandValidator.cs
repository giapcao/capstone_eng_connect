using EngConnect.Domain.Constants;
using FluentValidation;

namespace EngConnect.Application.UseCases.CourseEnrollments.UpdateCourseEnrollment;

public class UpdateCourseEnrollmentCommandValidator : AbstractValidator<UpdateCourseEnrollmentCommand>
{
    public UpdateCourseEnrollmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID đăng ký không được để trống");
        
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Mã khóa học không được để trống");
        
        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("Mã học sinh không được để trống");
        
        RuleFor(x => x.PriceAtPurchase)
            .GreaterThanOrEqualTo(1000).WithMessage("Giá mua không được nhỏ hơn 1000.")
            .When(x => x.PriceAtPurchase.HasValue);
        
        RuleFor(x => x.NumsOfSession)
            .GreaterThan(0).WithMessage("Số buổi học phải lớn hơn 0.")
            .When(x => x.NumsOfSession.HasValue);
        
    }
}
