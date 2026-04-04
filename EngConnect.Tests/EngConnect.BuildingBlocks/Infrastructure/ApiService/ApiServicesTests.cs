using System.Net;
using EngConnect.BuildingBlock.Contracts.Models.Email;
using EngConnect.BuildingBlock.Infrastructure.ApiService;
using EngConnect.Tests.Common;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace EngConnect.Tests.EngConnect.BuildingBlocks.Infrastructure.ApiService;

public class ApiServicesTests
{
    [Fact]
    public async Task Get_and_post_return_success_results()
    {
        var handler = new StubHttpMessageHandler((request, cancellationToken) =>
        {
            var body = request.Method == HttpMethod.Get ? "get-response" : "post-response";
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(body)
            });
        });

        var httpClient = new HttpClient(handler);
        var sut = new ApiServices(httpClient, NullLogger<ApiServices>.Instance);

        var getResult = await sut.GetAsync(new Uri("https://api.example/get"), headers => headers["x-test"] = "true");
        var postResult = await sut.PostAsync(new Uri("https://api.example/post"), new EmailContent(), headers => headers["x-test"] = "true");

        Assert.True(getResult.IsSuccess);
        Assert.Equal("get-response", getResult.Value);
        Assert.True(postResult.IsSuccess);
        Assert.Equal("post-response", postResult.Value);
    }
}
