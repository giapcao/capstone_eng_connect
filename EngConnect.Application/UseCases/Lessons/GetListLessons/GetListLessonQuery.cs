using EngConnect.Application.UseCases.Lessons.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.Lessons.GetListLessons;

public record GetListLessonQuery : BaseQuery<PaginationResult<GetLessonResponse>>
{
    public string? Status { get; set; }
    
    public Guid? TutorId { get; set; }
    
    public Guid? StudentId { get; set; }
    
    public DateTime? StartTimeFrom { get; set; }
    
    public DateTime? StartTimeTo { get; set; }
}
