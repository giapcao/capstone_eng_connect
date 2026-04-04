using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.GetFolder;

public class GetFolderValidatorTests
{
    [Theory]
    [MemberData(nameof(GetFolderTestData.NormalValidatorCases), MemberType = typeof(GetFolderTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(GetFolderTestData.Definition, caseSet);
    }
}