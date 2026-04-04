using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetListTutor;

public class GetListTutorBranchTests
{
    [Theory]
    [MemberData(nameof(GetListTutorTestData.HandlerBranchCases), MemberType = typeof(GetListTutorTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetListTutorTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListTutorTestData.Definition.ValidatorTypeFullName));
    }
}