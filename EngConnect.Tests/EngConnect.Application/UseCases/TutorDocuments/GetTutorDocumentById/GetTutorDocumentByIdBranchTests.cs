using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorDocuments.GetTutorDocumentById;

public class GetTutorDocumentByIdBranchTests
{
    [Theory]
    [MemberData(nameof(GetTutorDocumentByIdTestData.HandlerBranchCases), MemberType = typeof(GetTutorDocumentByIdTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetTutorDocumentByIdTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetTutorDocumentByIdTestData.Definition.ValidatorTypeFullName));
    }
}