using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument;

public class UploadTutorDocumentValidatorTests
{
    [Theory]
    [MemberData(nameof(UploadTutorDocumentTestData.NormalValidatorCases), MemberType = typeof(UploadTutorDocumentTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(UploadTutorDocumentTestData.Definition, caseSet);
    }
}