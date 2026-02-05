using EngConnect.Domain.Constants;
using FluentValidation;

namespace EngConnect.Application.UseCases.CourseEnrollments.GetListCourseEnrollments;

public class GetListCourseEnrollmentValidator : AbstractValidator<GetListCourseEnrollmentQuery>
{
    public GetListCourseEnrollmentValidator()
    {
        RuleFor(x => x.Status)
            .Must(status => string.IsNullOrEmpty(status) || Enum.TryParse<CourseEnrollmentStatus>(status, true, out _))
            .WithMessage("Trạng thái không hợp lệ");
    }
}
