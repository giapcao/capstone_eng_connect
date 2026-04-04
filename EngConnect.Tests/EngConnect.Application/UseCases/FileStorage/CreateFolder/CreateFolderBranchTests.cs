using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.CreateFolder;

public class CreateFolderBranchTests
{
    [Theory]
    [MemberData(nameof(CreateFolderTestData.HandlerBranchCases), MemberType = typeof(CreateFolderTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(CreateFolderTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(CreateFolderTestData.ValidatorBranchCases), MemberType = typeof(CreateFolderTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(CreateFolderTestData.Definition, caseSet);
    }
}