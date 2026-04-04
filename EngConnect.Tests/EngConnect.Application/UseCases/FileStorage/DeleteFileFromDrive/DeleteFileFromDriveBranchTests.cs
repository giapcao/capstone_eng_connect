using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.DeleteFileFromDrive;

public class DeleteFileFromDriveBranchTests
{
    [Theory]
    [MemberData(nameof(DeleteFileFromDriveTestData.HandlerBranchCases), MemberType = typeof(DeleteFileFromDriveTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteFileFromDriveTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(DeleteFileFromDriveTestData.ValidatorBranchCases), MemberType = typeof(DeleteFileFromDriveTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(DeleteFileFromDriveTestData.Definition, caseSet);
    }
}