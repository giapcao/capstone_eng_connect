using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.UserRoles.CreateUserRole;

public class CreateUserRoleBranchTests
{
    [Theory]
    [MemberData(nameof(CreateUserRoleTestData.HandlerBranchCases), MemberType = typeof(CreateUserRoleTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateUserRoleTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreateUserRoleTestData.ValidatorBranchCases), MemberType = typeof(CreateUserRoleTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateUserRoleTestData.Definition, caseSet);
    }
}