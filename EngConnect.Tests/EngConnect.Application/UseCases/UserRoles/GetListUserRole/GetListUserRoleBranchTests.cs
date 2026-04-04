using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.UserRoles.GetListUserRole;

public class GetListUserRoleBranchTests
{
    [Theory]
    [MemberData(nameof(GetListUserRoleTestData.HandlerBranchCases), MemberType = typeof(GetListUserRoleTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListUserRoleTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListUserRoleTestData.Definition.ValidatorTypeFullName));
    }
}