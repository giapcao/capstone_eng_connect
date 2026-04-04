using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Meetings.EndMeeting;

public class EndMeetingBranchTests
{
    [Theory]
    [MemberData(nameof(EndMeetingTestData.HandlerBranchCases), MemberType = typeof(EndMeetingTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(EndMeetingTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(EndMeetingTestData.Definition.ValidatorTypeFullName));
    }
}