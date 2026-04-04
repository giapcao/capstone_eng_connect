using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.DeleteCategory;

public class DeleteCategoryBranchTests
{
    [Theory]
    [MemberData(nameof(DeleteCategoryTestData.HandlerBranchCases), MemberType = typeof(DeleteCategoryTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteCategoryTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteCategoryTestData.Definition.ValidatorTypeFullName));
    }
}