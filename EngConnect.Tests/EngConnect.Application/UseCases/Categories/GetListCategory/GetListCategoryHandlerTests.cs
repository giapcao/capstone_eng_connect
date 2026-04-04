using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Categories.GetListCategory;

public class GetListCategoryHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListCategoryTestData.NormalHandlerCases), MemberType = typeof(GetListCategoryTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListCategoryTestData.Definition, caseSet);
    }
}