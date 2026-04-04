using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.CreateFolder;

public class CreateFolderValidatorTests
{
    [Theory]
    [MemberData(nameof(CreateFolderTestData.NormalValidatorCases), MemberType = typeof(CreateFolderTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateFolderTestData.Definition, caseSet);
    }
}