using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole;

public class CreatePermissionRoleBranchTests
{
    [Theory]
    [MemberData(nameof(CreatePermissionRoleTestData.HandlerBranchCases), MemberType = typeof(CreatePermissionRoleTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreatePermissionRoleTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreatePermissionRoleTestData.ValidatorBranchCases), MemberType = typeof(CreatePermissionRoleTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreatePermissionRoleTestData.Definition, caseSet);
    }
}