using EngConnect.Application.UseCases.CourseReviews.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseReviews.UpdateCourseReview;

public class UpdateCourseReviewCommand : ICommand<GetCourseReviewResponse>
{
    public required Guid Id { get; set; }
    public short? Rating { get; set; }
    public string? Comment { get; set; }
    public bool? IsAnonymous { get; set; }
}