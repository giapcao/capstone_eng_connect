using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetTutorById;

public class GetTutorByIdValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetTutorByIdTestData.Definition.ValidatorTypeFullName));
    }
}