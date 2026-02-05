using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.LessonScripts.CreateLessonScript;

public class CreateLessonScriptCommand : ICommand
{
    public Guid LessonId { get; set; }

    public Guid RecordId { get; set; }

    public string? Language { get; set; }

    public string? FullText { get; set; }

    public string? SummarizeText { get; set; }

    public string? LessonOutcome { get; set; }

    public decimal? CoveragePercent { get; set; }
}
