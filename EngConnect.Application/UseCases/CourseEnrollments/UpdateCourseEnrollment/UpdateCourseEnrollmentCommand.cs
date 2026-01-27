using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseEnrollments.UpdateCourseEnrollment;

public class UpdateCourseEnrollmentCommand : ICommand
{
    public Guid Id { get; set; }
    
    public Guid CourseId { get; set; }
    
    public Guid StudentId { get; set; }
    
    public decimal? PriceAtPurchase { get; set; }
    
    public int? NumsOfSession { get; set; }
}
