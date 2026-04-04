using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.GetPresignedUrl;

public class GetPresignedUrlValidatorTests
{
    [Theory]
    [MemberData(nameof(GetPresignedUrlTestData.NormalValidatorCases), MemberType = typeof(GetPresignedUrlTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(GetPresignedUrlTestData.Definition, caseSet);
    }
}