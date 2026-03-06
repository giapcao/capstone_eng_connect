using EngConnect.Application.UseCases.CourseVerificationRequests.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.CourseVerificationRequests.GetListCourseVerificationRequest;

public record GetListCourseVerificationRequestQuery : BaseQuery<PaginationResult<GetCourseVerificationRequestResponse>>
{
    public Guid? CourseId { get; set; }
    public string? Status { get; set; }
    public Guid? ReviewedBy { get; set; }
}
