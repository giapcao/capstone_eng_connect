using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.GetListCategory;

public class GetListCategoryBranchTests
{
    [Theory]
    [MemberData(nameof(GetListCategoryTestData.HandlerBranchCases), MemberType = typeof(GetListCategoryTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListCategoryTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListCategoryTestData.Definition.ValidatorTypeFullName));
    }
}