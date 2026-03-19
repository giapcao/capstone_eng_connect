using System.Text.Json.Serialization;

namespace EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;

public class AnalysisResponse
{
    public string AiSummarizeText { get; set; } = string.Empty;
    public DetailResult Detail { get; set; } = new();
    public decimal CoveragePercent { get; set; }
}