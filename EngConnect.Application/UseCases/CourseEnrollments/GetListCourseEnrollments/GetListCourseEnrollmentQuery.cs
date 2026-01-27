using EngConnect.Application.UseCases.CourseEnrollments.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.CourseEnrollments.GetListCourseEnrollments;

public record GetListCourseEnrollmentQuery : BaseQuery<PaginationResult<GetCourseEnrollmentResponse>>
{
    public string? Status { get; set; }
    
    public Guid? CourseId { get; set; }
    
    public Guid? StudentId { get; set; }
    
    public DateTime? EnrolledFrom { get; set; }
    
    public DateTime? EnrolledTo { get; set; }
    
    public DateTime? ExpiredFrom { get; set; }
    
    public DateTime? ExpiredTo { get; set; }
    
    
}
