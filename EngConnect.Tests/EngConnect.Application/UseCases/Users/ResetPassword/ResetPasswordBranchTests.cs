using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.ResetPassword;

public class ResetPasswordBranchTests
{
    [Theory]
    [MemberData(nameof(ResetPasswordTestData.HandlerBranchCases), MemberType = typeof(ResetPasswordTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(ResetPasswordTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(ResetPasswordTestData.ValidatorBranchCases), MemberType = typeof(ResetPasswordTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(ResetPasswordTestData.Definition, caseSet);
    }
}