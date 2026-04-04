using System.Text.Json;
using EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.Domain.Constants;
using Mscc.GenerativeAI;

namespace EngConnect.Infrastructure.AiService;

public class AiSummarizeService
{
    private readonly GenerativeModel _model;

    public AiSummarizeService(IGenerativeAI model)
    {
        _model = model.GenerativeModel("gemini-2.0-flash");
    }


    public async Task<AnalysisResponse?> AnalyzeContentAsync(AnalysisRequest request)
    {
        var generateConfig = new GenerateContentConfig
        {
            ResponseMimeType = "application/json",
        };
        var fullPrompt = string.Format(SummarizePromptConstant.SummarizePromptTemplate, request.Transcript,
            request.Outcome);
        try
        {
            var response = await _model.GenerateContent(fullPrompt, generateConfig);
            if (ValidationUtil.IsNullOrEmpty(response.Text))
            {
                return null;
            }

            var result = JsonSerializer.Deserialize<AnalysisResponse>(response.Text, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi rồi: {ex.Message}");
            return null;
        }
    }

    public async Task<GenerateQuizResponse?> GenerateQuizAsync(GenerateQuizRequest request)
    {
        var generateConfig = new GenerateContentConfig
        {
            ResponseMimeType = "application/json",
        };
        var fullPrompt = GenerateQuizConstant.GeneratePromptTemplate.Replace("{{CONTENT}}", request.Content);
        try
        {
            var response = await _model.GenerateContent(fullPrompt, generateConfig);
            if (ValidationUtil.IsNullOrEmpty(response.Text))
            {
                return null;
            }

            var questions = JsonSerializer.Deserialize<List<QuizQuestionResponse>>(response.Text, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            return new GenerateQuizResponse
            {
                Questions = questions ?? []
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi tạo quiz: {ex.Message}");
            return null;
        }
    }
}