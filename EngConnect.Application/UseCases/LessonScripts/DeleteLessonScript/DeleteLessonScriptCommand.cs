using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.LessonScripts.DeleteLessonScript;

public record DeleteLessonScriptCommand : ICommand
{
    public Guid Id { get; set; }
}
