using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.UploadFileToDrive;

public class UploadFileToDriveBranchTests
{
    [Theory]
    [MemberData(nameof(UploadFileToDriveTestData.HandlerBranchCases), MemberType = typeof(UploadFileToDriveTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UploadFileToDriveTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UploadFileToDriveTestData.ValidatorBranchCases), MemberType = typeof(UploadFileToDriveTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UploadFileToDriveTestData.Definition, caseSet);
    }
}