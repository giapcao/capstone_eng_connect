using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.VerifyEmail;

public class VerifyEmailBranchTests
{
    [Theory]
    [MemberData(nameof(VerifyEmailTestData.HandlerBranchCases), MemberType = typeof(VerifyEmailTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(VerifyEmailTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(VerifyEmailTestData.ValidatorBranchCases), MemberType = typeof(VerifyEmailTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(VerifyEmailTestData.Definition, caseSet);
    }
}