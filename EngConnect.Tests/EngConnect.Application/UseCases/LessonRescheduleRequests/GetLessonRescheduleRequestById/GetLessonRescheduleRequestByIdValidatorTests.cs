using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRescheduleRequests.GetLessonRescheduleRequestById;

public class GetLessonRescheduleRequestByIdValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetLessonRescheduleRequestByIdTestData.Definition.ValidatorTypeFullName));
    }
}