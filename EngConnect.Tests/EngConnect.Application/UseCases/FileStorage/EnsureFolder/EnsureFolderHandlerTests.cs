using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.EnsureFolder;

public class EnsureFolderHandlerTests
{
    [Theory]
    [MemberData(nameof(EnsureFolderTestData.NormalHandlerCases), MemberType = typeof(EnsureFolderTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(EnsureFolderTestData.Definition, caseSet);
    }
}