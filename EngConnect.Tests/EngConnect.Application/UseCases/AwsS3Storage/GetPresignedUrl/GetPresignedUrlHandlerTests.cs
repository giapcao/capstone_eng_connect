using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.GetPresignedUrl;

public class GetPresignedUrlHandlerTests
{
    [Theory]
    [MemberData(nameof(GetPresignedUrlTestData.NormalHandlerCases), MemberType = typeof(GetPresignedUrlTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetPresignedUrlTestData.Definition, caseSet);
    }
}