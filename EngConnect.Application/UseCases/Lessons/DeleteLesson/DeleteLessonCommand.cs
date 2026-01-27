using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.Lessons.DeleteLesson;

public record DeleteLessonCommand : ICommand
{
    public Guid Id { get; set; }
}
