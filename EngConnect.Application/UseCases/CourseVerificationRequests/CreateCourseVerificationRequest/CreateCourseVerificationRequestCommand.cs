using System.Text.Json.Serialization;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseVerificationRequests.CreateCourseVerificationRequest;

public class CreateCourseVerificationRequestCommand : ICommand
{
    [JsonIgnore]
    public Guid TutorId { get; set; }
    public required Guid CourseId { get; set; }
}
