using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorDocuments.GetListTutorDocument;

public class GetListTutorDocumentBranchTests
{
    [Theory]
    [MemberData(nameof(GetListTutorDocumentTestData.HandlerBranchCases), MemberType = typeof(GetListTutorDocumentTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListTutorDocumentTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListTutorDocumentTestData.Definition.ValidatorTypeFullName));
    }
}