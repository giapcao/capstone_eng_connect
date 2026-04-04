using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Meetings.GetMeetingInfo;

public class GetMeetingInfoBranchTests
{
    [Theory]
    [MemberData(nameof(GetMeetingInfoTestData.HandlerBranchCases), MemberType = typeof(GetMeetingInfoTestData))]
    public async Task HandleAsync_covers_branch_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetMeetingInfoTestData.Definition, caseSet);
    }

    [Fact]
    public void ValidateAsync_has_no_validator_branch_cases()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetMeetingInfoTestData.Definition.ValidatorTypeFullName));
    }
}