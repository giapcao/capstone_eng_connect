using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.DownloadFile;

public class DownloadFileBranchTests
{
    [Theory]
    [MemberData(nameof(DownloadFileTestData.HandlerBranchCases), MemberType = typeof(DownloadFileTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DownloadFileTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(DownloadFileTestData.ValidatorBranchCases), MemberType = typeof(DownloadFileTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(DownloadFileTestData.Definition, caseSet);
    }
}