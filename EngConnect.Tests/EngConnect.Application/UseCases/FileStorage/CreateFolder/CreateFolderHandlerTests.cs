using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.CreateFolder;

public class CreateFolderHandlerTests
{
    [Theory]
    [MemberData(nameof(CreateFolderTestData.NormalHandlerCases), MemberType = typeof(CreateFolderTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateFolderTestData.Definition, caseSet);
    }
}