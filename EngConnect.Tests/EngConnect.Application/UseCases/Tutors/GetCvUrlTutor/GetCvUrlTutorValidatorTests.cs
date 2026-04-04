using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetCvUrlTutor;

public class GetCvUrlTutorValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetCvUrlTutorTestData.Definition.ValidatorTypeFullName));
    }
}