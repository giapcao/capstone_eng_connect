using FluentValidation;

namespace EngConnect.Application.UseCases.CourseReviews.UpdateCourseReview;

public class UpdateCourseReviewCommandValidator : AbstractValidator<UpdateCourseReviewCommand>
{
    public UpdateCourseReviewCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống");

        RuleFor(x => x.Rating)
            .InclusiveBetween((short)1, (short)5).WithMessage("Rating phải từ 1 đến 5")
            .When(x => x.Rating.HasValue);

        RuleFor(x => x.Comment)
            .MaximumLength(1000).WithMessage("Comment không được vượt quá 1000 ký tự")
            .When(x => !string.IsNullOrEmpty(x.Comment));
    }
}