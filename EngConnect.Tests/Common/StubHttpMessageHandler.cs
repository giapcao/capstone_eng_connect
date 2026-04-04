using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace EngConnect.Tests.Common;

internal sealed class StubHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handler;

    public StubHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>? handler = null)
    {
        _handler = handler ?? DefaultHandlerAsync;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return _handler(request, cancellationToken);
    }

    public static HttpResponseMessage CreateJsonResponse(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
    }

    private static Task<HttpResponseMessage> DefaultHandlerAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri?.AbsolutePath.Contains("transcribe", StringComparison.OrdinalIgnoreCase) == true)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("transcribed-text", Encoding.UTF8, "text/plain")
            });
        }

        const string body =
            """
            {
              "choices": [
                {
                  "message": {
                    "content": "{\"aiSummarizeText\":\"summary\",\"detail\":{\"pass\":[],\"fail\":[]},\"coveragePercent\":100}"
                  }
                }
              ]
            }
            """;

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return Task.FromResult(response);
    }
}
