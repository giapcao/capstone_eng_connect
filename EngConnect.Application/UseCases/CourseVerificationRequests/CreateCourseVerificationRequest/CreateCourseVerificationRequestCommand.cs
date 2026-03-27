using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseVerificationRequests.CreateCourseVerificationRequest;

public class CreateCourseVerificationRequestCommand : ICommand
{
    public required Guid CourseId { get; set; }
}
