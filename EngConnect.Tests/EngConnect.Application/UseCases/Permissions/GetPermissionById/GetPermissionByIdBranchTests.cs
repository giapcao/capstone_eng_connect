using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Permissions.GetPermissionById;

public class GetPermissionByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetPermissionByIdTestData.HandlerBranchCases), MemberType = typeof(GetPermissionByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetPermissionByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetPermissionByIdTestData.Definition.ValidatorTypeFullName));
    }
}