using System.Net;
using EngConnect.BuildingBlock.Contracts.Models.Files;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.Infrastructure.AiService;
using EngConnect.Tests.Common;
using Microsoft.Extensions.Options;
using Xunit;

namespace EngConnect.Tests.EngConnect.Infrastructure.AiService;

public class AiWhisperTranscriptTests
{
    [Fact]
    public async Task Transcribe_returns_response_content()
    {
        var httpClient = TestObjectFactory.CreateHttpClient((request, cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("transcribed")
            }));

        var settings = Options.Create(new WhisperApiSettings
        {
            Endpoint = "https://speech.example/"
        });

        var sut = new AiWhisperTranscript(httpClient, settings);

        var result = await sut.Transcribe(new FileUpload
        {
            FileName = "audio.mp3",
            ContentType = "audio/mpeg",
            Length = 10,
            Content = new MemoryStream([1, 2, 3])
        });

        Assert.Equal("transcribed", result);
    }
}
