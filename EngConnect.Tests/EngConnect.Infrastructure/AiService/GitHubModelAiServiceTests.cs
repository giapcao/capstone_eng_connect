using EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.Infrastructure.AiService;
using EngConnect.Tests.Common;
using Microsoft.Extensions.Options;
using Xunit;

namespace EngConnect.Tests.EngConnect.Infrastructure.AiService;

public class GitHubModelAiServiceTests
{
    [Fact]
    public async Task AnalyzeContentAsync_deserializes_analysis_response()
    {
        const string responseBody =
            """
            {
              "choices": [
                {
                  "message": {
                    "content": "{\"aiSummarizeText\":\"summary\",\"detail\":{\"pass\":[\"p1\"],\"fail\":[]},\"coveragePercent\":87.5}"
                  }
                }
              ]
            }
            """;

        var httpClient = TestObjectFactory.CreateHttpClient((request, cancellationToken) =>
            Task.FromResult(StubHttpMessageHandler.CreateJsonResponse(responseBody)));

        var settings = Options.Create(new GitHubModelsSettings
        {
            Endpoint = "https://models.example/",
            Token = "token",
            ModelName = "model"
        });

        var sut = new GitHubModelAiService(httpClient, settings);

        var result = await sut.AnalyzeContentAsync(new AnalysisRequest("transcript", "outcome"));

        Assert.NotNull(result);
        Assert.Equal("summary", result!.AiSummarizeText);
        Assert.Equal(87.5m, result.CoveragePercent);
        Assert.Equal("Bearer", httpClient.DefaultRequestHeaders.Authorization?.Scheme);
    }
}
