using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument;

public class RemoveTutorDocumentValidatorTests
{
    [Theory]
    [MemberData(nameof(RemoveTutorDocumentTestData.NormalValidatorCases), MemberType = typeof(RemoveTutorDocumentTestData))]
    public async Task ValidateAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertValidatorCaseAsync(RemoveTutorDocumentTestData.Definition, caseSet);
    }
}