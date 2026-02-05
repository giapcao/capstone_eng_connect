using EngConnect.Application.UseCases.LessonRecords.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.LessonRecords.GetListLessonRecords;

public record GetListLessonRecordQuery : BaseQuery<PaginationResult<GetLessonRecordResponse>>
{
    public Guid? LessonId { get; set; }
}
