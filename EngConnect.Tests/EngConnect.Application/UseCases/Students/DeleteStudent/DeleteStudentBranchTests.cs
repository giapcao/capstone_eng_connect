using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.DeleteStudent;

public class DeleteStudentBranchTests
{
    [Theory]
    [MemberData(nameof(DeleteStudentTestData.HandlerBranchCases), MemberType = typeof(DeleteStudentTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(DeleteStudentTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteStudentTestData.Definition.ValidatorTypeFullName));
    }
}