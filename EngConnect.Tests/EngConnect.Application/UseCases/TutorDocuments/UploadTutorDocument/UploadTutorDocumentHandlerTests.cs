using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument;

public class UploadTutorDocumentHandlerTests
{
    [Theory]
    [MemberData(nameof(UploadTutorDocumentTestData.NormalHandlerCases), MemberType = typeof(UploadTutorDocumentTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UploadTutorDocumentTestData.Definition, caseSet);
    }
}