using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.ForgotPassword;

public class ForgotPasswordBranchTests
{
    [Theory]
    [MemberData(nameof(ForgotPasswordTestData.HandlerBranchCases), MemberType = typeof(ForgotPasswordTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(ForgotPasswordTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(ForgotPasswordTestData.ValidatorBranchCases), MemberType = typeof(ForgotPasswordTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(ForgotPasswordTestData.Definition, caseSet);
    }
}