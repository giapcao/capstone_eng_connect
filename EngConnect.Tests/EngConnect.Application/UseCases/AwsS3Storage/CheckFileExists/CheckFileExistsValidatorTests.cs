using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.CheckFileExists;

public class CheckFileExistsValidatorTests
{
    [Theory]
    [MemberData(nameof(CheckFileExistsTestData.NormalValidatorCases), MemberType = typeof(CheckFileExistsTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CheckFileExistsTestData.Definition, caseSet);
    }
}