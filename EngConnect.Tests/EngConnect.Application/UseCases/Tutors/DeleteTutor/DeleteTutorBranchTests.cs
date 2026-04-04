using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.DeleteTutor;

public class DeleteTutorBranchTests
{
    [Theory]
    [MemberData(nameof(DeleteTutorTestData.HandlerBranchCases), MemberType = typeof(DeleteTutorTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteTutorTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteTutorTestData.Definition.ValidatorTypeFullName));
    }
}