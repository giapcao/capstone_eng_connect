using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRescheduleRequests.GetListLessonRescheduleRequest;

public class GetListLessonRescheduleRequestValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListLessonRescheduleRequestTestData.Definition.ValidatorTypeFullName));
    }
}