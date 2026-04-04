using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.GetListUser;

public class GetListUserBranchTests
{
    [Theory]
    [MemberData(nameof(GetListUserTestData.HandlerBranchCases), MemberType = typeof(GetListUserTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListUserTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListUserTestData.Definition.ValidatorTypeFullName));
    }
}