namespace EngConnect.Application.UseCases.CourseEnrollments.Common;

public class GetCourseEnrollmentResponse
{
    public Guid Id { get; set; }
    
    public Guid CourseId { get; set; }
    
    public Guid StudentId { get; set; }
    
    public decimal? PriceAtPurchase { get; set; }
    
    public string? Currency { get; set; }
    
    public int? NumsOfSession { get; set; }
    
    public string? Status { get; set; }
    
    public DateTime? EnrolledAt { get; set; }
    
    public DateTime? ExpiredAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}
