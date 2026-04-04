using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.TutorSchedules.GetTutorScheduleById;

public class GetTutorScheduleByIdValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetTutorScheduleByIdTestData.Definition.ValidatorTypeFullName));
    }
}