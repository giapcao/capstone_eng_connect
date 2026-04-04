using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.LoginByUser;

public class LoginByUserBranchTests
{
    [Theory]
    [MemberData(nameof(LoginByUserTestData.HandlerBranchCases), MemberType = typeof(LoginByUserTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(LoginByUserTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(LoginByUserTestData.ValidatorBranchCases), MemberType = typeof(LoginByUserTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(LoginByUserTestData.Definition, caseSet);
    }
}