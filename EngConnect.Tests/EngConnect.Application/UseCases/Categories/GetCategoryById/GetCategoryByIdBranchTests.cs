using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.GetCategoryById;

public class GetCategoryByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetCategoryByIdTestData.HandlerBranchCases), MemberType = typeof(GetCategoryByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetCategoryByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetCategoryByIdTestData.Definition.ValidatorTypeFullName));
    }
}