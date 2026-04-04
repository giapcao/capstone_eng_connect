using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorDocuments.GetListTutorDocument;

public class GetListTutorDocumentHandlerTests
{
    [Theory]
    [MemberData(nameof(GetListTutorDocumentTestData.NormalHandlerCases), MemberType = typeof(GetListTutorDocumentTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListTutorDocumentTestData.Definition, caseSet);
    }
}