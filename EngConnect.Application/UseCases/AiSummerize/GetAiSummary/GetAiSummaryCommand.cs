using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;

namespace EngConnect.Application.UseCases.AiSummerize.GetAiSummary;

public record GetAiSummaryCommand : ICommand<AnalysisResponse>
{
    public Guid LessonId { get; set; }
    public required string Transcript { get; set; }
}
