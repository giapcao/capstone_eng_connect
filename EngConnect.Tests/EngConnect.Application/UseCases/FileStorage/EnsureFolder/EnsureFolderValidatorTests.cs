using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.EnsureFolder;

public class EnsureFolderValidatorTests
{
    [Theory]
    [MemberData(nameof(EnsureFolderTestData.NormalValidatorCases), MemberType = typeof(EnsureFolderTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(EnsureFolderTestData.Definition, caseSet);
    }
}