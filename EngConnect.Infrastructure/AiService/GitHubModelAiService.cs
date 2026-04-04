using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.AiSummerzie;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.Domain.Constants; 
using Microsoft.Extensions.Options;
using Mscc.GenerativeAI;

namespace EngConnect.Infrastructure.AiService;

public class GitHubModelAiService : IAiService
{
    private readonly HttpClient _httpClient;
    private readonly GitHubModelsSettings _settings;

    public GitHubModelAiService(HttpClient httpClient, IOptions<GitHubModelsSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
        _httpClient.BaseAddress = new Uri(_settings.Endpoint);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.Token);
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("EngConnectApp"); 
    }
    
    public async Task<AnalysisResponse?> AnalyzeContentAsync(AnalysisRequest request)
    {
        
        var fullPrompt = string.Format(SummarizePromptConstant.SummarizePromptTemplate, 
            request.Transcript, request.Outcome);
        
        var requestBody = new
        {
            model = _settings.ModelName,
            messages = new[]
            {
                new { role = "system", content = "You are a helpful assistant that strictly outputs JSON." },
                new { role = "user", content = fullPrompt }
            },
            response_format = new { type = "json_object" } 
        };

        try
        {
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("", content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            
            using var doc = JsonDocument.Parse(jsonResponse);
            var aiTextResponse = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (ValidationUtil.IsNullOrEmpty(aiTextResponse)) 
                return null;
            
            return JsonSerializer.Deserialize<AnalysisResponse>(aiTextResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GitHub Model Error: {ex.Message}");
            return null;
        }
    }
    
   public async Task<GenerateQuizResponse?> GenerateQuizAsync(GenerateQuizRequest request)
    {
        var fullPrompt = GenerateQuizConstant.GeneratePromptTemplate.Replace("{{CONTENT}}", request.Content);
        
        var requestBody = new
        {
            model = _settings.ModelName,
            messages = new[]
            {
                new { role = "system", content = "You are a helpful assistant that strictly outputs JSON. Your JSON must have a root key named 'questions' which is an array of quiz objects." },
                new { role = "user", content = fullPrompt }
            },
            response_format = new { type = "json_object" } 
        };

        try
        {
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("", content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            
            using var doc = JsonDocument.Parse(jsonResponse);
            var aiTextResponse = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (string.IsNullOrEmpty(aiTextResponse)) return null;
            
            using var responseDoc = JsonDocument.Parse(aiTextResponse);
            var questionsElement = responseDoc.RootElement.GetProperty("questions").GetRawText();

            var questions = JsonSerializer.Deserialize<List<QuizQuestionResponse>>(questionsElement, new JsonSerializerOptions
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
            Console.WriteLine($"GitHub Model Error - GenerateQuiz: {ex}");
            return null;
        }
    }
}