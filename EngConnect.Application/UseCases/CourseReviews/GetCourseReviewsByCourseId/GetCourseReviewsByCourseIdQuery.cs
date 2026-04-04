using EngConnect.Application.UseCases.CourseReviews.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.CourseReviews.GetCourseReviewsByCourseId;

public record GetCourseReviewsByCourseIdQuery : BaseQuery<PaginationResult<GetCourseReviewResponse>>
{
    public Guid? CourseId { get; set; }
}