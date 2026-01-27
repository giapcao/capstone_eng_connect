using EngConnect.Application.UseCases.LessonRecords.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.LessonRecords.GetLessonRecordById;

public record GetLessonRecordByIdQuery : IQuery<GetLessonRecordResponse>
{
    public Guid Id { get; set; }
}
