using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.ChangePassword;

public class ChangePasswordBranchTests
{
    [Theory]
    [MemberData(nameof(ChangePasswordTestData.HandlerBranchCases), MemberType = typeof(ChangePasswordTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(ChangePasswordTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(ChangePasswordTestData.ValidatorBranchCases), MemberType = typeof(ChangePasswordTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(ChangePasswordTestData.Definition, caseSet);
    }
}