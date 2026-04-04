using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument;

public class UploadTutorDocumentBranchTests
{
    [Theory]
    [MemberData(nameof(UploadTutorDocumentTestData.HandlerBranchCases), MemberType = typeof(UploadTutorDocumentTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UploadTutorDocumentTestData.Definition, caseSet);
    }

    [Theory]
    [MemberData(nameof(UploadTutorDocumentTestData.ValidatorBranchCases), MemberType = typeof(UploadTutorDocumentTestData))]
    public async Task ValidateAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UploadTutorDocumentTestData.Definition, caseSet);
    }
}