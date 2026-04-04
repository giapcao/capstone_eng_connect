using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.UploadFile;

public class UploadFileBranchTests
{
    [Theory]
    [MemberData(nameof(UploadFileTestData.HandlerBranchCases), MemberType = typeof(UploadFileTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UploadFileTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UploadFileTestData.ValidatorBranchCases), MemberType = typeof(UploadFileTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UploadFileTestData.Definition, caseSet);
    }
}