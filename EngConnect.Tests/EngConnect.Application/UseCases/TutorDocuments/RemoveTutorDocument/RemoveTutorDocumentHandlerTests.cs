using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument;

public class RemoveTutorDocumentHandlerTests
{
    [Theory]
    [MemberData(nameof(RemoveTutorDocumentTestData.NormalHandlerCases), MemberType = typeof(RemoveTutorDocumentTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(RemoveTutorDocumentTestData.Definition, caseSet);
    }
}