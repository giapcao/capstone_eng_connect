using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;

namespace EngConnect.Application.UseCases.LessonScripts.GenerateQuizByLessonScriptId;

public record GenerateQuizByLessonScriptIdQuery : IQuery<GenerateQuizResponse>
{
    public required Guid LessonScriptId { get; set; }
}