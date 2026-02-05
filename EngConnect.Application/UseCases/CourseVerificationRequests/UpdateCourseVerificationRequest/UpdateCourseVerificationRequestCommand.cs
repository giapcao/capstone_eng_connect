using System.Text.Json.Serialization;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.CourseVerificationRequests.UpdateCourseVerificationRequest;

public class UpdateCourseVerificationRequestCommand : ICommand
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string? Status { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public Guid? ReviewedBy { get; set; }
    public string? RejectionReason { get; set; }
}
