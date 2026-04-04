using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.GetFileFromDrive;

public class GetFileFromDriveValidatorTests
{
    [Theory]
    [MemberData(nameof(GetFileFromDriveTestData.NormalValidatorCases), MemberType = typeof(GetFileFromDriveTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(GetFileFromDriveTestData.Definition, caseSet);
    }
}