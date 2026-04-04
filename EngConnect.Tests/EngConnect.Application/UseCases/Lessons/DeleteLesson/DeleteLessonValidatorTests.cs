using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Lessons.DeleteLesson;

public class DeleteLessonValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(DeleteLessonTestData.Definition.ValidatorTypeFullName));
    }
}