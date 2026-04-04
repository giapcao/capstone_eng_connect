using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.DownloadFile;

public class DownloadFileValidatorTests
{
    [Theory]
    [MemberData(nameof(DownloadFileTestData.NormalValidatorCases), MemberType = typeof(DownloadFileTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(DownloadFileTestData.Definition, caseSet);
    }
}