using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.DeleteFile;

public class DeleteFileBranchTests
{
    [Theory]
    [MemberData(nameof(DeleteFileTestData.HandlerBranchCases), MemberType = typeof(DeleteFileTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteFileTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(DeleteFileTestData.ValidatorBranchCases), MemberType = typeof(DeleteFileTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(DeleteFileTestData.Definition, caseSet);
    }
}