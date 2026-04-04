using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.UpdateCategory;

public class UpdateCategoryBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateCategoryTestData.HandlerBranchCases), MemberType = typeof(UpdateCategoryTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateCategoryTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UpdateCategoryTestData.ValidatorBranchCases), MemberType = typeof(UpdateCategoryTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateCategoryTestData.Definition, caseSet);
    }
}