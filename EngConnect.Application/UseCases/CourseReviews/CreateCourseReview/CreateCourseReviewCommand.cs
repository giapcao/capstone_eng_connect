using EngConnect.Application.UseCases.CourseReviews.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseReviews.CreateCourseReview;

public class CreateCourseReviewCommand : ICommand
{
    public required Guid CourseId { get; set; }
    public required Guid StudentId { get; set; }
    public required Guid TutorId {get;set;}
    public required Guid EnrollmentId { get; set; }
    public short? Rating { get; set; }
    public string? Comment { get; set; }
    public bool? IsAnonymous { get; set; }
}