using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.LessonRecords.DeleteLessonRecord;

public record DeleteLessonRecordCommand : ICommand
{
    public Guid Id { get; set; }
}
