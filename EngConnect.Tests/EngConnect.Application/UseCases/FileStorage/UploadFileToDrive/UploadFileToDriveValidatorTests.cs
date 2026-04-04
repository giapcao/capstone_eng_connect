using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.UploadFileToDrive;

public class UploadFileToDriveValidatorTests
{
    [Theory]
    [MemberData(nameof(UploadFileToDriveTestData.NormalValidatorCases), MemberType = typeof(UploadFileToDriveTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UploadFileToDriveTestData.Definition, caseSet);
    }
}