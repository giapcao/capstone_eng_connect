using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Students.GetAvatarStudent;

public class GetAvatarStudentBranchTests
{
    [Theory]
    [MemberData(nameof(GetAvatarStudentTestData.HandlerBranchCases), MemberType = typeof(GetAvatarStudentTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetAvatarStudentTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetAvatarStudentTestData.Definition.ValidatorTypeFullName));
    }
}