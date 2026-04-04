using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.DeleteFolder;

public class DeleteFolderHandlerTests
{
    [Theory]
    [MemberData(nameof(DeleteFolderTestData.NormalHandlerCases), MemberType = typeof(DeleteFolderTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteFolderTestData.Definition, caseSet);
    }
}