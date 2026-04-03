using FluentValidation;

namespace EngConnect.Application.UseCases.CourseReviews.CreateCourseReview;

public class CreateCourseReviewCommandValidator : AbstractValidator<CreateCourseReviewCommand>
{
    public CreateCourseReviewCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("CourseId không được để trống");

        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("StudentId không được để trống");

        RuleFor(x => x.EnrollmentId)
            .NotEmpty().WithMessage("EnrollmentId không được để trống");
        RuleFor(x => x.TutorId)
            .NotEmpty().WithMessage("TutorId không được để trống");

        RuleFor(x => x.Rating)
            .InclusiveBetween((short)1, (short)5).WithMessage("Rating phải từ 1 đến 5")
            .When(x => x.Rating.HasValue);

        RuleFor(x => x.Comment)
            .MaximumLength(1000).WithMessage("Comment không được vượt quá 1000 ký tự")
            .When(x => !string.IsNullOrEmpty(x.Comment));
    }
}