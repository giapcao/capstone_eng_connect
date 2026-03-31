using System.Text.Json.Serialization;
using EngConnect.Application.UseCases.CourseVerificationRequests.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.CourseVerificationRequests.GetListCourseVerificationRequest;

public record GetListCourseVerificationRequestQuery : BaseQuery<PaginationResult<GetCourseVerificationRequestResponse>>
{
    [JsonIgnore]
    public Guid? TutorId { get; set; }
    public Guid? CourseId { get; set; }
    public string? Status { get; set; }
    public Guid? ReviewedBy { get; set; }
}
