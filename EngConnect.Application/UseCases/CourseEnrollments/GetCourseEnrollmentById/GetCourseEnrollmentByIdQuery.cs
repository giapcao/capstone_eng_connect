using EngConnect.Application.UseCases.CourseEnrollments.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseEnrollments.GetCourseEnrollmentById;

public record GetCourseEnrollmentByIdQuery : IQuery<GetCourseEnrollmentResponse>
{
    public Guid Id { get; set; }
}
