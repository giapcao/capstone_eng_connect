using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.GetFileFromDrive;

public class GetFileFromDriveBranchTests
{
    [Theory]
    [MemberData(nameof(GetFileFromDriveTestData.HandlerBranchCases), MemberType = typeof(GetFileFromDriveTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetFileFromDriveTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(GetFileFromDriveTestData.ValidatorBranchCases), MemberType = typeof(GetFileFromDriveTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(GetFileFromDriveTestData.Definition, caseSet);
    }
}