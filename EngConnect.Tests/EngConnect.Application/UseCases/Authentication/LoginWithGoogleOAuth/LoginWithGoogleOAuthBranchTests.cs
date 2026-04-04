using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.LoginWithGoogleOAuth;

public class LoginWithGoogleOAuthBranchTests
{
    [Theory]
    [MemberData(nameof(LoginWithGoogleOAuthTestData.HandlerBranchCases), MemberType = typeof(LoginWithGoogleOAuthTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(LoginWithGoogleOAuthTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(LoginWithGoogleOAuthTestData.Definition.ValidatorTypeFullName));
    }
}