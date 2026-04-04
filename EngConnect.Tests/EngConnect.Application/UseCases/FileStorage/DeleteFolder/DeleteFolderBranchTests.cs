using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.DeleteFolder;

public class DeleteFolderBranchTests
{
    [Theory]
    [MemberData(nameof(DeleteFolderTestData.HandlerBranchCases), MemberType = typeof(DeleteFolderTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteFolderTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(DeleteFolderTestData.ValidatorBranchCases), MemberType = typeof(DeleteFolderTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(DeleteFolderTestData.Definition, caseSet);
    }
}