using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.DeleteCategory;

public class DeleteCategoryHandlerTests
{
    [Theory]
    [MemberData(nameof(DeleteCategoryTestData.NormalHandlerCases), MemberType = typeof(DeleteCategoryTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteCategoryTestData.Definition, caseSet);
    }
}