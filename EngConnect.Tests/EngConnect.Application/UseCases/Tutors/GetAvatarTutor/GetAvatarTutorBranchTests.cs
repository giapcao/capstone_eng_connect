using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetAvatarTutor;

public class GetAvatarTutorBranchTests
{
    [Theory]
    [MemberData(nameof(GetAvatarTutorTestData.HandlerBranchCases), MemberType = typeof(GetAvatarTutorTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetAvatarTutorTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetAvatarTutorTestData.Definition.ValidatorTypeFullName));
    }
}