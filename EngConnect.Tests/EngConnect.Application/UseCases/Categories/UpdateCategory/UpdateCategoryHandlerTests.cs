using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.UpdateCategory;

public class UpdateCategoryHandlerTests
{
    [Theory]
    [MemberData(nameof(UpdateCategoryTestData.NormalHandlerCases), MemberType = typeof(UpdateCategoryTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateCategoryTestData.Definition, caseSet);
    }
}