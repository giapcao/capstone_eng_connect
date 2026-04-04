using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonScripts.GetLessonScriptById;

public class GetLessonScriptByIdValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetLessonScriptByIdTestData.Definition.ValidatorTypeFullName));
    }
}