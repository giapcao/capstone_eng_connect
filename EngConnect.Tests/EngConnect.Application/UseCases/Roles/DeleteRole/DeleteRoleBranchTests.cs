using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Roles.DeleteRole;

public class DeleteRoleBranchTests
{
    [Theory]
    [MemberData(nameof(DeleteRoleTestData.HandlerBranchCases), MemberType = typeof(DeleteRoleTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteRoleTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteRoleTestData.Definition.ValidatorTypeFullName));
    }
}