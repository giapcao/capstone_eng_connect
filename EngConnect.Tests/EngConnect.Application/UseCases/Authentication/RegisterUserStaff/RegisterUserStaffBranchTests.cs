using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Authentication.RegisterUserStaff;

public class RegisterUserStaffBranchTests
{
    [Theory]
    [MemberData(nameof(RegisterUserStaffTestData.HandlerBranchCases), MemberType = typeof(RegisterUserStaffTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(RegisterUserStaffTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(RegisterUserStaffTestData.ValidatorBranchCases), MemberType = typeof(RegisterUserStaffTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(RegisterUserStaffTestData.Definition, caseSet);
    }
}