using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseEnrollments.CreateCourseEnrollment;

public class CreateCourseEnrollmentCommand : ICommand
{
    public Guid CourseId { get; set; }
    
    public Guid StudentId { get; set; }
    
    public decimal? PriceAtPurchase { get; set; }
    
    public int? NumsOfSession { get; set; }
    
    public DateTime? ExpiredAt { get; set; }
}
