using System.Net;
using System.Text;
using System.Text.Json;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.BuildingBlock.Infrastructure.ApiService;

public class ApiServices
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiServices> _logger;

    public ApiServices(HttpClient httpClient, ILogger<ApiServices> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Result<string>> GetAsync(Uri uri, Action<Dictionary<string, string>> headers)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            Dictionary<string, string> header = new();
            headers(header);
            foreach (var item in header)
            {
                request.Headers.Add(item.Key, item.Value);
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // get the response content
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return Result.Failure<string>(HttpStatusCode.InternalServerError, ApiErrors.ApiCallFailed);
            }

            return Result.Success(content);
        }
        catch (Exception e)
        {
            // Log error
            _logger.LogError("Error calling API: {Message}", e.Message);
            return Result.Failure<string>(HttpStatusCode.InternalServerError, ApiErrors.ApiCallFailed);
        }
    }

    public async Task<Result<string>> PostAsync(Uri uri, object data, Action<Dictionary<string, string>> headers)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            Dictionary<string, string> header = new();
            headers(header);
            foreach (var item in header)
            {
                request.Headers.Add(item.Key, item.Value);
            }

            var requestContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            request.Content = requestContent;
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            // get the response content
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return Result.Failure<string>(HttpStatusCode.InternalServerError, ApiErrors.ApiCallFailed);
            }

            return Result.Success(content);
        }
        catch (Exception e)
        {
            // Log error
            _logger.LogError("Error calling API: {Message}", e.Message);
            return Result.Failure<string>(HttpStatusCode.InternalServerError, ApiErrors.ApiCallFailed);
        }
    }
}