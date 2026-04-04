using Xunit;

namespace EngConnect.Tests.EngConnect.Application.UseCases.Tutors.GetListTutor;

public class GetListTutorValidatorTests
{
    [Fact]
    public void ValidateAsync_has_no_validator_for_this_usecase()
    {
        Assert.True(string.IsNullOrWhiteSpace(GetListTutorTestData.Definition.ValidatorTypeFullName));
    }
}