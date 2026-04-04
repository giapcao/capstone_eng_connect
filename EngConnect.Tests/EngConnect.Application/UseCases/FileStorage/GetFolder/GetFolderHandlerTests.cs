using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.GetFolder;

public class GetFolderHandlerTests
{
    [Theory]
    [MemberData(nameof(GetFolderTestData.NormalHandlerCases), MemberType = typeof(GetFolderTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetFolderTestData.Definition, caseSet);
    }
}