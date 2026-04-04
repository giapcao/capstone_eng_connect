using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.Logout;

public class LogoutBranchTests
{
    [Theory]
    [MemberData(nameof(LogoutTestData.HandlerBranchCases), MemberType = typeof(LogoutTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(LogoutTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(LogoutTestData.Definition.ValidatorTypeFullName));
    }
}