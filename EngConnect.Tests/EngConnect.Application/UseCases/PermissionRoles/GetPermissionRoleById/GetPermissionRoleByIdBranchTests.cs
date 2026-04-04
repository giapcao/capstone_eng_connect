using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.PermissionRoles.GetPermissionRoleById;

public class GetPermissionRoleByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetPermissionRoleByIdTestData.HandlerBranchCases), MemberType = typeof(GetPermissionRoleByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetPermissionRoleByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetPermissionRoleByIdTestData.Definition.ValidatorTypeFullName));
    }
}