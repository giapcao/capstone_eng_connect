using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.GetCategoryById;

public class GetCategoryByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetCategoryByIdTestData.NormalHandlerCases), MemberType = typeof(GetCategoryByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetCategoryByIdTestData.Definition, caseSet);
    }
}