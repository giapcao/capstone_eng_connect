using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RegisterUser;

public class RegisterUserBranchTests
{
    [Theory]
    [MemberData(nameof(RegisterUserTestData.HandlerBranchCases), MemberType = typeof(RegisterUserTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(RegisterUserTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(RegisterUserTestData.ValidatorBranchCases), MemberType = typeof(RegisterUserTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(RegisterUserTestData.Definition, caseSet);
    }
}