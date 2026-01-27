using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseEnrollments.DeleteCourseEnrollment;

public record DeleteCourseEnrollmentCommand : ICommand
{
    public Guid Id { get; set; }
}
