using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.GetLessonById;

public class GetLessonByIdValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetLessonByIdTestData.Definition.ValidatorTypeFullName));
    }
}