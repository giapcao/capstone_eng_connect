using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.UserRoles.GetUserRoleById;

public class GetUserRoleByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetUserRoleByIdTestData.HandlerBranchCases), MemberType = typeof(GetUserRoleByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetUserRoleByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetUserRoleByIdTestData.Definition.ValidatorTypeFullName));
    }
}