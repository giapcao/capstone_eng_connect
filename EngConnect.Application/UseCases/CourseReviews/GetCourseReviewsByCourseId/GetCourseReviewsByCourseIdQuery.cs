using EngConnect.Application.UseCases.CourseReviews.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseReviews.GetCourseReviewsByCourseId;

public class GetCourseReviewsByCourseIdQuery : IQuery<List<GetCourseReviewResponse>>
{
    public required Guid CourseId { get; set; }
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
}