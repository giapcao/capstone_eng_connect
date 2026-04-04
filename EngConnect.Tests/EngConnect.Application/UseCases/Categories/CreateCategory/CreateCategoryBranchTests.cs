using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.CreateCategory;

public class CreateCategoryBranchTests
{
    [Theory]
    [MemberData(nameof(CreateCategoryTestData.HandlerBranchCases), MemberType = typeof(CreateCategoryTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateCategoryTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreateCategoryTestData.ValidatorBranchCases), MemberType = typeof(CreateCategoryTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateCategoryTestData.Definition, caseSet);
    }
}