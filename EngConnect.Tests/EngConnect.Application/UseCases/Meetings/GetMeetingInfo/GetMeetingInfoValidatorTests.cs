using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Meetings.GetMeetingInfo;

public class GetMeetingInfoValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetMeetingInfoTestData.Definition.ValidatorTypeFullName));
    }
}