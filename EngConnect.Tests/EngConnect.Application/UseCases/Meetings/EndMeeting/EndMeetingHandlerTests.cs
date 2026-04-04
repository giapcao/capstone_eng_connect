using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Meetings.EndMeeting;

public class EndMeetingHandlerTests
{
    [Theory]
    [MemberData(nameof(EndMeetingTestData.NormalHandlerCases), MemberType = typeof(EndMeetingTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(EndMeetingTestData.Definition, caseSet);
    }
}