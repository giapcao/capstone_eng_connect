using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.GetPresignedUrl;

public class GetPresignedUrlBranchTests
{
    [Theory]
    [MemberData(nameof(GetPresignedUrlTestData.HandlerBranchCases), MemberType = typeof(GetPresignedUrlTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetPresignedUrlTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(GetPresignedUrlTestData.ValidatorBranchCases), MemberType = typeof(GetPresignedUrlTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(GetPresignedUrlTestData.Definition, caseSet);
    }
}