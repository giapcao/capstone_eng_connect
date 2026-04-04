using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Roles.GetRoleById;

public class GetRoleByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetRoleByIdTestData.HandlerBranchCases), MemberType = typeof(GetRoleByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetRoleByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetRoleByIdTestData.Definition.ValidatorTypeFullName));
    }
}