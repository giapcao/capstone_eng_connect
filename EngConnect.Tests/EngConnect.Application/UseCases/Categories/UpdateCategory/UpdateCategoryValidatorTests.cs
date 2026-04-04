using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.UpdateCategory;

public class UpdateCategoryValidatorTests
{
    [Theory]
    [MemberData(nameof(UpdateCategoryTestData.NormalValidatorCases), MemberType = typeof(UpdateCategoryTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UpdateCategoryTestData.Definition, caseSet);
    }
}