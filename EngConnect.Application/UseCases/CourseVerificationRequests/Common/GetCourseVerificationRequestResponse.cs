namespace EngConnect.Application.UseCases.CourseVerificationRequests.Common;

public class GetCourseVerificationRequestResponse
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string? Status { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public Guid? ReviewedBy { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
