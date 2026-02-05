using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseEnrollments.UpdateCourseEnrollmentStatus;

public record UpdateCourseEnrollmentStatusCommand : ICommand
{
    public Guid Id { get; set; }
    
    public string? NewStatus { get; set; }
}