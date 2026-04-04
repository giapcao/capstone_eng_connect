namespace EngConnect.Application.UseCases.CourseReviews.Common;

public class GetCourseReviewResponse
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public Guid TutorId { get; set; }
    public Guid StudentId { get; set; }
    public Guid EnrollmentId { get; set; }
    public short? Rating { get; set; }
    public string? Comment { get; set; }
    public bool? IsAnonymous { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}