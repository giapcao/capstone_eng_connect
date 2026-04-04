using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.GetListPermission;

public class GetListPermissionBranchTests
{
    [Theory]
    [MemberData(nameof(GetListPermissionTestData.HandlerBranchCases), MemberType = typeof(GetListPermissionTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListPermissionTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListPermissionTestData.Definition.ValidatorTypeFullName));
    }
}