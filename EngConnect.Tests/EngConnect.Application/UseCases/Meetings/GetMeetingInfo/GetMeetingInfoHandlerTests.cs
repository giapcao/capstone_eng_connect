using EngConnect.Tests.Common;
using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Meetings.GetMeetingInfo;

public class GetMeetingInfoHandlerTests
{
    [Theory]
    [MemberData(nameof(GetMeetingInfoTestData.NormalHandlerCases), MemberType = typeof(GetMeetingInfoTestData))]
    public async Task HandleAsync_covers_normal_cases(UseCaseCaseSet caseSet)
    {
        await UseCaseTestHarness.AssertHandlerCaseAsync(GetMeetingInfoTestData.Definition, caseSet);
    }
}