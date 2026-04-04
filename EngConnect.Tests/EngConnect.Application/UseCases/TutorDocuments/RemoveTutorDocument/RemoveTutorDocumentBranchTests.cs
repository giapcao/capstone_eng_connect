using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument;

public class RemoveTutorDocumentBranchTests
{
    [Theory]
    [MemberData(nameof(RemoveTutorDocumentTestData.HandlerBranchCases), MemberType = typeof(RemoveTutorDocumentTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(RemoveTutorDocumentTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(RemoveTutorDocumentTestData.ValidatorBranchCases), MemberType = typeof(RemoveTutorDocumentTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(RemoveTutorDocumentTestData.Definition, caseSet);
    }
}