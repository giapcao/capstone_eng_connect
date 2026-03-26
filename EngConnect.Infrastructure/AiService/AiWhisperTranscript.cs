using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Settings;
using Microsoft.Extensions.Options;

namespace EngConnect.Infrastructure.AiService;

public class AiWhisperTranscript : IWhisperApiService
{
    private readonly HttpClient _httpClient;
    
    public AiWhisperTranscript(HttpClient httpClient, IOptions<WhisperApiSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.Endpoint);
        
        _httpClient.Timeout = TimeSpan.FromMinutes(5);
    }

    public async Task<string?> Transcribe(FileUpload fileUpload)
    {
        try
        {
            using var content = new MultipartFormDataContent();

            var fileContent = new StreamContent(fileUpload.Content);

            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/mpeg");

            content.Add(fileContent, "file", fileUpload.FileName);

            var response = await _httpClient.PostAsync("transcribe", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
       
    }
}