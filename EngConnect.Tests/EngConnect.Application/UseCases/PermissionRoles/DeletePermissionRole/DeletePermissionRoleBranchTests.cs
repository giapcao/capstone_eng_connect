using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.PermissionRoles.DeletePermissionRole;

public class DeletePermissionRoleBranchTests
{
    [Theory]
    [MemberData(nameof(DeletePermissionRoleTestData.HandlerBranchCases), MemberType = typeof(DeletePermissionRoleTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeletePermissionRoleTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeletePermissionRoleTestData.Definition.ValidatorTypeFullName));
    }
}