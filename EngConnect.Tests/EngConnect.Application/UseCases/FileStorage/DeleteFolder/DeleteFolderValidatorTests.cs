using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.DeleteFolder;

public class DeleteFolderValidatorTests
{
    [Theory]
    [MemberData(nameof(DeleteFolderTestData.NormalValidatorCases), MemberType = typeof(DeleteFolderTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(DeleteFolderTestData.Definition, caseSet);
    }
}