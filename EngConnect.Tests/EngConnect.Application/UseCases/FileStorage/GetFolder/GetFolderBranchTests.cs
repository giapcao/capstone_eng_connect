using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.GetFolder;

public class GetFolderBranchTests
{
    [Theory]
    [MemberData(nameof(GetFolderTestData.HandlerBranchCases), MemberType = typeof(GetFolderTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetFolderTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(GetFolderTestData.ValidatorBranchCases), MemberType = typeof(GetFolderTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(GetFolderTestData.Definition, caseSet);
    }
}