using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.UserRoles.DeleteUserRole;

public class DeleteUserRoleBranchTests
{
    [Theory]
    [MemberData(nameof(DeleteUserRoleTestData.HandlerBranchCases), MemberType = typeof(DeleteUserRoleTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteUserRoleTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteUserRoleTestData.Definition.ValidatorTypeFullName));
    }
}