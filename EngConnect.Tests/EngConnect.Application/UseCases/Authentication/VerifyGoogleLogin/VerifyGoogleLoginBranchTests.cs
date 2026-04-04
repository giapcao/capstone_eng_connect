using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.VerifyGoogleLogin;

public class VerifyGoogleLoginBranchTests
{
    [Theory]
    [MemberData(nameof(VerifyGoogleLoginTestData.HandlerBranchCases), MemberType = typeof(VerifyGoogleLoginTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(VerifyGoogleLoginTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(VerifyGoogleLoginTestData.Definition.ValidatorTypeFullName));
    }
}