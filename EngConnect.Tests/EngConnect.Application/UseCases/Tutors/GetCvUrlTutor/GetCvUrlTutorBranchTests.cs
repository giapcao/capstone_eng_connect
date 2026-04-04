using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetCvUrlTutor;

public class GetCvUrlTutorBranchTests
{
    [Theory]
    [MemberData(nameof(GetCvUrlTutorTestData.HandlerBranchCases), MemberType = typeof(GetCvUrlTutorTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetCvUrlTutorTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetCvUrlTutorTestData.Definition.ValidatorTypeFullName));
    }
}