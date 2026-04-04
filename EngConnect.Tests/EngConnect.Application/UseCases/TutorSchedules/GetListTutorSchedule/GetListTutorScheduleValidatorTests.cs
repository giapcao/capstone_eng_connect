using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.GetListTutorSchedule;

public class GetListTutorScheduleValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListTutorScheduleTestData.Definition.ValidatorTypeFullName));
    }
}