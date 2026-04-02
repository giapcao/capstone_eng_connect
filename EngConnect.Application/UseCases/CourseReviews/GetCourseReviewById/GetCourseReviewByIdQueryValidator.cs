using FluentValidation;

namespace EngConnect.Application.UseCases.CourseReviews.GetCourseReviewById;

public class GetCourseReviewByIdQueryValidator : AbstractValidator<GetCourseReviewByIdQuery>
{
    public GetCourseReviewByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống");
    }
}