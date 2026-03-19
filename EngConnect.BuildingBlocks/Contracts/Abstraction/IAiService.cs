using EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;

namespace EngConnect.BuildingBlock.Contracts.Abstraction;

public interface IAiService
{
    Task<AnalysisResponse?> AnalyzeContentAsync(AnalysisRequest request);
}