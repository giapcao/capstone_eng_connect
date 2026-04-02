using FluentValidation;

namespace EngConnect.Application.UseCases.CourseReviews.DeleteCourseReview;

public class DeleteCourseReviewCommandValidator : AbstractValidator<DeleteCourseReviewCommand>
{
    public DeleteCourseReviewCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống");
    }
}