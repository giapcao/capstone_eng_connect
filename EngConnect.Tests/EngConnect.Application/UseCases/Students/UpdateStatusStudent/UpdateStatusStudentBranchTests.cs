using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.UpdateStatusStudent;

public class UpdateStatusStudentBranchTests
{
    [Theory]
    [MemberData(nameof(UpdateStatusStudentTestData.HandlerBranchCases), MemberType = typeof(UpdateStatusStudentTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(UpdateStatusStudentTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(UpdateStatusStudentTestData.Definition.ValidatorTypeFullName));
    }
}