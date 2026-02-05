namespace EngConnect.Application.UseCases.LessonScripts.Common;

public class GetLessonScriptResponse
{
    public Guid Id { get; set; }
    
    public Guid LessonId { get; set; }

    public Guid RecordId { get; set; }

    public string? Language { get; set; }

    public string? FullText { get; set; }

    public string? SummarizeText { get; set; }

    public string? LessonOutcome { get; set; }

    public decimal? CoveragePercent { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}
