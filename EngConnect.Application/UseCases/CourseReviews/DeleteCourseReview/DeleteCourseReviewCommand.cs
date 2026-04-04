using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseReviews.DeleteCourseReview;

public record DeleteCourseReviewCommand(Guid Id) : ICommand;