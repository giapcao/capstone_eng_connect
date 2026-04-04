using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.LessonRecords.GetLessonRecordById;

public class GetLessonRecordByIdValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetLessonRecordByIdTestData.Definition.ValidatorTypeFullName));
    }
}