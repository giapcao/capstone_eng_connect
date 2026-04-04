using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.AwsS3Storage.CheckFileExists;

public class CheckFileExistsBranchTests
{
    [Theory]
    [MemberData(nameof(CheckFileExistsTestData.HandlerBranchCases), MemberType = typeof(CheckFileExistsTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CheckFileExistsTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CheckFileExistsTestData.ValidatorBranchCases), MemberType = typeof(CheckFileExistsTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CheckFileExistsTestData.Definition, caseSet);
    }
}