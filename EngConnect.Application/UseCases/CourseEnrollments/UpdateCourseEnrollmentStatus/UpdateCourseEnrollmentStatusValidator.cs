using EngConnect.Domain.Constants;
using FluentValidation;

namespace EngConnect.Application.UseCases.CourseEnrollments.UpdateCourseEnrollmentStatus;

public class UpdateCourseEnrollmentStatusValidator : AbstractValidator<UpdateCourseEnrollmentStatusCommand>
{
    public UpdateCourseEnrollmentStatusValidator()
    {
        RuleFor(x => x.NewStatus)
            .Must(status => string.IsNullOrEmpty(status) || Enum.TryParse<CourseEnrollmentStatus>(status, true, out _))
            .WithMessage("Trạng thái không hợp lệ");
        
    }
  
}