using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.PermissionRoles.GetListPermissionRole;

public class GetListPermissionRoleBranchTests
{
    [Theory]
    [MemberData(nameof(GetListPermissionRoleTestData.HandlerBranchCases), MemberType = typeof(GetListPermissionRoleTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListPermissionRoleTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListPermissionRoleTestData.Definition.ValidatorTypeFullName));
    }
}