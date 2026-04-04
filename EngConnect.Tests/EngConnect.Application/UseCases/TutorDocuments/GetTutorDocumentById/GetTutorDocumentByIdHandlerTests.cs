using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorDocuments.GetTutorDocumentById;

public class GetTutorDocumentByIdHandlerTests
{
    [Theory]
    [MemberData(nameof(GetTutorDocumentByIdTestData.NormalHandlerCases), MemberType = typeof(GetTutorDocumentByIdTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetTutorDocumentByIdTestData.Definition, caseSet);
    }
}