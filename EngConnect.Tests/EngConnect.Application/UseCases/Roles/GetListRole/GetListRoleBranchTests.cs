using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Roles.GetListRole;

public class GetListRoleBranchTests
{
    [Theory]
    [MemberData(nameof(GetListRoleTestData.HandlerBranchCases), MemberType = typeof(GetListRoleTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListRoleTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListRoleTestData.Definition.ValidatorTypeFullName));
    }
}