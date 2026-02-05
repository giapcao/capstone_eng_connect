using EngConnect.Application.UseCases.LessonScripts.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.LessonScripts.GetLessonScriptById;

public record GetLessonScriptByIdQuery : IQuery<GetLessonScriptResponse>
{
    public Guid Id { get; set; }
}
