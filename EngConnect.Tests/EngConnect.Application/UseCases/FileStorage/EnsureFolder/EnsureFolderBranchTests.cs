using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.FileStorage.EnsureFolder;

public class EnsureFolderBranchTests
{
    [Theory]
    [MemberData(nameof(EnsureFolderTestData.HandlerBranchCases), MemberType = typeof(EnsureFolderTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(EnsureFolderTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(EnsureFolderTestData.ValidatorBranchCases), MemberType = typeof(EnsureFolderTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(EnsureFolderTestData.Definition, caseSet);
    }
}