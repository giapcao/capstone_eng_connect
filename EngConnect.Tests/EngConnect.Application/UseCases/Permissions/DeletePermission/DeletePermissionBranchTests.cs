using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.DeletePermission;

public class DeletePermissionBranchTests
{
    [Theory]
    [MemberData(nameof(DeletePermissionTestData.HandlerBranchCases), MemberType = typeof(DeletePermissionTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeletePermissionTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeletePermissionTestData.Definition.ValidatorTypeFullName));
    }
}