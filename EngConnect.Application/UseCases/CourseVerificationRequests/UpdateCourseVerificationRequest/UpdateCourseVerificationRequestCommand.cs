using System.Text.Json.Serialization;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseVerificationRequests.UpdateCourseVerificationRequest;

public class UpdateCourseVerificationRequestCommand : ICommand
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    public Guid Id { get; set; }
    public string? Status { get; set; }
    public string? RejectionReason { get; set; }
}
