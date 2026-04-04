using EngConnect.Application.UseCases.CourseReviews.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseReviews.GetCourseReviewById;

public class GetCourseReviewByIdQuery : IQuery<GetCourseReviewResponse>
{
    public required Guid Id { get; set; }
}