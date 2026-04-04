using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Users.GetUserById;

public class GetUserByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetUserByIdTestData.HandlerBranchCases), MemberType = typeof(GetUserByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetUserByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetUserByIdTestData.Definition.ValidatorTypeFullName));
    }
}