using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive;

public class DeleteFileFromDriveValidatorTests
{
    [Theory]
    [MemberData(nameof(DeleteFileFromDriveTestData.NormalValidatorCases), MemberType = typeof(DeleteFileFromDriveTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(DeleteFileFromDriveTestData.Definition, caseSet);
    }
}