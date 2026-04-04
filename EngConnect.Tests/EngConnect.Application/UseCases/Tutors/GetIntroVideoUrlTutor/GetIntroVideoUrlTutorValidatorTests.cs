using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetIntroVideoUrlTutor;

public class GetIntroVideoUrlTutorValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetIntroVideoUrlTutorTestData.Definition.ValidatorTypeFullName));
    }
}