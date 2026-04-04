using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.CreateCategory;

public class CreateCategoryValidatorTests
{
    [Theory]
    [MemberData(nameof(CreateCategoryTestData.NormalValidatorCases), MemberType = typeof(CreateCategoryTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateCategoryTestData.Definition, caseSet);
    }
}