using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Meetings.EndMeeting;

public class EndMeetingValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(EndMeetingTestData.Definition.ValidatorTypeFullName));
    }
}