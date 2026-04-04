using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.DownloadFile;

public class DownloadFileHandlerTests
{
    [Theory]
    [MemberData(nameof(DownloadFileTestData.NormalHandlerCases), MemberType = typeof(DownloadFileTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DownloadFileTestData.Definition, caseSet);
    }
}