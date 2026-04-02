using FluentValidation;

namespace EngConnect.Application.UseCases.CourseReviews.GetCourseReviewsByCourseId;

public class GetCourseReviewsByCourseIdQueryValidator : AbstractValidator<GetCourseReviewsByCourseIdQuery>
{
    public GetCourseReviewsByCourseIdQueryValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("CourseId không được để trống");

        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page phải lớn hơn 0")
            .When(x => x.Page.HasValue);

        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("PageSize phải lớn hơn 0")
            .LessThanOrEqualTo(100).WithMessage("PageSize không được vượt quá 100")
            .When(x => x.PageSize.HasValue);
    }
}