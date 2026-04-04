using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.CreateCategory;

public class CreateCategoryHandlerTests
{
    [Theory]
    [MemberData(nameof(CreateCategoryTestData.NormalHandlerCases), MemberType = typeof(CreateCategoryTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateCategoryTestData.Definition, caseSet);
    }
}