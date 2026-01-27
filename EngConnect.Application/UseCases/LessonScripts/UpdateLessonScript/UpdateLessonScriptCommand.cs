using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.LessonScripts.UpdateLessonScript;

public class UpdateLessonScriptCommand : ICommand
{
    public Guid Id { get; set; }
    
    public Guid LessonId { get; set; }

    public Guid RecordId { get; set; }

    public string? Language { get; set; }

    public string? FullText { get; set; }

    public string? SummarizeText { get; set; }

    public string? LessonOutcome { get; set; }

    public decimal? CoveragePercent { get; set; }
}
